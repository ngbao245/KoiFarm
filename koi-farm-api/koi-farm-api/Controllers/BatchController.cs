using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing;
using Repository.Data.Entity;
using Repository.Model;
using Repository.Model.Batch;
using Repository.Repository;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class BatchController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork;

    public BatchController(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    [HttpGet("get-all-batches")]
    public IActionResult GetAllBatches()
    {
        var batches = _unitOfWork.BatchRepository.Get(includeProperties: b => b.batchItems).ToList();
        if (!batches.Any())
        {
            return NotFound(new ResponseModel
            {
                StatusCode = 404,
                MessageError = "No batches found."
            });
        }

        return Ok(new ResponseModel
        {
            StatusCode = 200,
            Data = batches.Select(batch => new BatchResponseModel
            {
                Id = batch.Id,
                Name = batch.Name,
                Price = batch.Price,
                Description = batch.Description,
                Quantity = batch.Quantity,
                ImageUrl = batch.ImageUrl,
                Items = batch.batchItems.Select(item => new BatchItemResponseModel
                {
                    BatchItemId = item.Id,
                    Name = item.Name,
                    Sex = item.Sex,
                    Age = item.Age,
                    Size = item.Size,
                    ImageUrl = item.ImageUrl,
                    Quantity = item.Quantity,
                }).ToList()
            }).ToList()
        });
    }

    [HttpGet("get-batch/{id}")]
    public IActionResult GetBatchById(string id)
    {
        var batch = _unitOfWork.BatchRepository.Get(
            c => c.Id == id,
            includeProperties: c => c.batchItems
        ).FirstOrDefault();

        if (batch == null)
        {
            return NotFound(new ResponseModel
            {
                StatusCode = 404,
                MessageError = "Batch not found"
            });
        }

        var response = new
        {
            Id = batch.Id,
            Name = batch.Name,
            Price = batch.Price,
            Description = batch.Description,
            Quantity = batch.Quantity,
            ImageUrl = batch.ImageUrl,
            Items = batch.batchItems.Select(item => new BatchItemResponseModel
            {
                BatchItemId = item.Id,
                Name = item.Name,
                Sex = item.Sex,
                Age = item.Age,
                Size = item.Size,
                ImageUrl = item.ImageUrl,
                Quantity = item.Quantity,
            }).ToList()
        };

        return Ok(new ResponseModel
        {
            StatusCode = 200,
            Data = response
        });
    }

    [Authorize(Roles = "Manager,Staff")]
    [HttpPost("create-batch")]
    public IActionResult CreateBatch([FromBody] RequestCreateBatchModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResponseModel
            {
                StatusCode = 400,
                MessageError = "Invalid input data"
            });
        }

        if (model.Name == null || model.Description == null || model.ImageUrl == null)
        {
            return BadRequest(new ResponseModel
            {
                StatusCode = 400,
                MessageError = "Every field is required!"
            });
        }

        var batch = new Batch
        {
            Name = model.Name,
            Description = model.Description,
            ImageUrl = model.ImageUrl,
            Price = 0,
            Quantity = 0
        };

        _unitOfWork.BatchRepository.Create(batch);

        var response = new
        {
            Name = batch.Name,
            Description = batch.Description,
            ImageUrl = batch.ImageUrl,
            Price = batch.Price,
            Quantity = batch.Quantity
        };

        return Ok(new ResponseModel
        {
            StatusCode = 201,
            Data = response
        });
    }

    [Authorize(Roles = "Manager,Staff")]
    [HttpPut("update-batch/{id}")]
    public IActionResult UpdateBatch(string id, [FromBody] RequestCreateBatchModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResponseModel
            {
                StatusCode = 400,
                MessageError = "Invalid input data"
            });
        }

        var existingBatch = _unitOfWork.BatchRepository.GetById(id);
        if (existingBatch == null)
        {
            return NotFound(new ResponseModel
            {
                StatusCode = 404,
                MessageError = "Batch not found."
            });
        }

        if (model.Name == null || model.Description == null || model.ImageUrl == null)
        {
            return BadRequest(new ResponseModel
            {
                StatusCode = 400,
                MessageError = "Every field is required!"
            });
        }

        if (model.Name != null) existingBatch.Name = model.Name;
        if (model.Description != null) existingBatch.Description = model.Description;
        if (model.ImageUrl != null) existingBatch.ImageUrl = model.ImageUrl;

        _unitOfWork.BatchRepository.Update(existingBatch);

        return Ok(new ResponseModel
        {
            StatusCode = 200,
            Data = new
            {
                Name = existingBatch.Name,
                Description = existingBatch.Description,
                ImageUrl = existingBatch.ImageUrl,
                Price = existingBatch.Price,
                Quantity = existingBatch.Quantity
            }
        });
    }


    [Authorize(Roles = "Manager,Staff")]
    [HttpDelete("delete-batch/{id}")]
    public IActionResult DeleteBatch(string id)
    {
        var existingBatch = _unitOfWork.BatchRepository.GetById(id);
        if (existingBatch == null)
        {
            return NotFound(new ResponseModel
            {
                StatusCode = 404,
                MessageError = "Batch not found."
            });
        }

        var batchItems = _unitOfWork.ProductItemRepository.Get().Where(i => i.BatchId == id);
        if (batchItems.Any())
        {
            return Conflict(new ResponseModel
            {
                StatusCode = 409,
                MessageError = "Cannot remove batch because some productItems are using this batch, remove items first to delete batch."
            });
        }

        _unitOfWork.BatchRepository.Delete(existingBatch);

        return Ok(new ResponseModel
        {
            StatusCode = 200,
            MessageError = "Batch successfully deleted."
        });
    }


    [Authorize(Roles = "Manager,Staff")]
    [HttpPost("add-item-to-batch/{batchId}")]
    public IActionResult AddItemToBatch(string batchId, [FromBody] RequestAddBatchItemModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResponseModel
            {
                StatusCode = 400,
                MessageError = "Invalid input data"
            });
        }

        var batch = _unitOfWork.BatchRepository.GetById(batchId);
        if (batch == null)
        {
            return NotFound(new ResponseModel
            {
                StatusCode = 404,
                MessageError = "Batch not found."
            });
        }

        var productItem = _unitOfWork.ProductItemRepository.GetById(model.ProductItemId);
        if (productItem == null)
        {
            return NotFound(new ResponseModel
            {
                StatusCode = 404,
                MessageError = "Product item not found."
            });
        }

        if (!string.IsNullOrEmpty(productItem.BatchId))
        {
            return Conflict(new ResponseModel
            {
                StatusCode = 409,
                MessageError = "Product item is already assigned to a batch."
            });
        }

        productItem.BatchId = batchId;
        _unitOfWork.ProductItemRepository.Update(productItem);

        batch.Quantity++;
        batch.Price += productItem.Price;
        _unitOfWork.BatchRepository.Update(batch);

        return Ok(new ResponseModel
        {
            StatusCode = 200,
            MessageError = "Product item successfully added to the batch."
        });
    }

    [Authorize(Roles = "Manager,Staff")]
    [HttpDelete("remove-item-from-batch/{batchId}/{productItemId}")]
    public IActionResult RemoveItemFromBatch(string batchId, string productItemId)
    {
        var batch = _unitOfWork.BatchRepository.GetById(batchId);
        if (batch == null)
        {
            return NotFound(new ResponseModel
            {
                StatusCode = 404,
                MessageError = "Batch not found."
            });
        }

        var productItem = _unitOfWork.ProductItemRepository.GetById(productItemId);
        if (productItem == null || productItem.BatchId != batchId)
        {
            return NotFound(new ResponseModel
            {
                StatusCode = 404,
                MessageError = "Product item not found in this batch."
            });
        }

        productItem.BatchId = null;
        _unitOfWork.ProductItemRepository.Update(productItem);

        batch.Quantity--;
        batch.Price -= productItem.Price;
        _unitOfWork.BatchRepository.Update(batch);

        return Ok(new ResponseModel
        {
            StatusCode = 200,
            MessageError = "Product item successfully removed from the batch."
        });
    }
}
