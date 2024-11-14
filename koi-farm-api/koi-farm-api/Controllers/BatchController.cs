//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.FileSystemGlobbing;
//using Repository.Data.Entity;
//using Repository.Model;
//using Repository.Model.Batch;
//using Repository.Repository;
//using System.Linq;

//[ApiController]
//[Route("api/[controller]")]
//public class BatchController : ControllerBase
//{
//    private readonly UnitOfWork _unitOfWork;

//    public BatchController(UnitOfWork unitOfWork)
//    {
//        _unitOfWork = unitOfWork;
//    }

//    // Get all batches without filtering
//    [HttpGet("all-batches")]
//    public IActionResult GetAllBatches()
//    {
//        var batches = _unitOfWork.BatchRepository.Get(includeProperties: b => b.batchItems).ToList();

//        if (!batches.Any())
//        {
//            return NotFound(new ResponseModel
//            {
//                StatusCode = 404,
//                MessageError = "No batches found"
//            });
//        }

//        var response = batches.Select(batch => new
//        {
//            BatchId = batch.Id,
//            BatchName = batch.name,
//            Description = batch.Description,
//            Quantity = batch.Quantity,
//            TotalPrice = batch.batchItems.Sum(item => item.Price),
//            ImageUrls = batch.batchItems.Select(item => item.ImageUrl).Where(url => url != null).ToList(),
//            ProductItems = batch.batchItems.Select(item => new
//            {
//                ProductItemId = item.Id,
//                Name = item.Name,
//                Price = item.Price,
//                ImageUrl = item.ImageUrl
//            }).ToList()
//        }).ToList();

//        return Ok(new ResponseModel
//        {
//            StatusCode = 200,
//            Data = response
//        });
//    }

//    // Get batches filtered by productId
//    [HttpGet("batches-by-product/{productId}")]
//    public IActionResult GetBatchesByProductId(string productId)
//    {
//        if (string.IsNullOrEmpty(productId))
//        {
//            return BadRequest(new ResponseModel
//            {
//                StatusCode = 400,
//                MessageError = "The productId field is required."
//            });
//        }

//        var batches = _unitOfWork.BatchRepository.Get(b => b.ProductId == productId, includeProperties: b => b.batchItems).ToList();

//        if (!batches.Any())
//        {
//            return NotFound(new ResponseModel
//            {
//                StatusCode = 404,
//                MessageError = "No batches found for the specified productId"
//            });
//        }

//        var response = batches.Select(batch => new
//        {
//            BatchId = batch.Id,
//            BatchName = batch.name,
//            Description = batch.Description,
//            Quantity = batch.Quantity,
//            TotalPrice = batch.batchItems.Sum(item => item.Price),
//            ImageUrls = batch.batchItems.Select(item => item.ImageUrl).Where(url => url != null).ToList(),
//            ProductItems = batch.batchItems.Select(item => new
//            {
//                ProductItemId = item.Id,
//                Name = item.Name,
//                Price = item.Price,
//                ImageUrl = item.ImageUrl
//            }).ToList()
//        }).ToList();

//        return Ok(new ResponseModel
//        {
//            StatusCode = 200,
//            Data = response
//        });
//    }

//    // Get batch by ID
//    [HttpGet("batch/{id}")]
//    public IActionResult GetBatchById(string id)
//    {
//        var batch = _unitOfWork.BatchRepository.Get(b => b.Id == id, includeProperties: b => b.batchItems).FirstOrDefault();

//        if (batch == null)
//        {
//            return NotFound(new ResponseModel
//            {
//                StatusCode = 404,
//                MessageError = "Batch not found"
//            });
//        }

//        var response = new
//        {
//            BatchId = batch.Id,
//            BatchName = batch.name,
//            Description = batch.Description,
//            Quantity = batch.Quantity,
//            TotalPrice = batch.batchItems.Sum(item => item.Price),
//            ImageUrls = batch.batchItems.Select(item => item.ImageUrl).Where(url => url != null).ToList(),
//            ProductItems = batch.batchItems.Select(item => new
//            {
//                ProductItemId = item.Id,
//                Name = item.Name,
//                Price = item.Price,
//                ImageUrl = item.ImageUrl
//            }).ToList()
//        };

//        return Ok(new ResponseModel
//        {
//            StatusCode = 200,
//            Data = response
//        });
//    }

//    // Get batch by name
//    [HttpGet("batch-by-name/{name}")]
//    public IActionResult GetBatchByName(string name)
//    {
//        var batch = _unitOfWork.BatchRepository.Get(b => b.name.Contains(name), includeProperties: b => b.batchItems).ToList();

//        if (batch == null)
//        {
//            return NotFound(new ResponseModel
//            {
//                StatusCode = 404,
//                MessageError = "Batch not found"
//            });
//        }

//        var response = batch.Select(batch => new
//        {
//            BatchId = batch.Id,
//            BatchName = batch.name,
//            Description = batch.Description,
//            Quantity = batch.Quantity,
//            TotalPrice = batch.batchItems.Sum(item => item.Price),
//            ImageUrls = batch.batchItems.Select(item => item.ImageUrl).Where(url => url != null).ToList(),
//            ProductItems = batch.batchItems.Select(item => new
//            {
//                ProductItemId = item.Id,
//                Name = item.Name,
//                Price = item.Price,
//                ImageUrl = item.ImageUrl
//            }).ToList()
//        }).ToList();

//        return Ok(new ResponseModel
//        {
//            StatusCode = 200,
//            Data = response
//        });
//    }


//    // Get list of ProductItem image URLs
//    [HttpGet("product-item-images/{batchId}")]
//    public IActionResult GetProductItemImages(string batchId)
//    {
//        var batch = _unitOfWork.BatchRepository.Get(b => b.Id == batchId, includeProperties: b => b.batchItems).FirstOrDefault();

//        if (batch == null)
//        {
//            return NotFound(new ResponseModel
//            {
//                StatusCode = 404,
//                MessageError = "Batch not found"
//            });
//        }

//        // Filter out null ImageUrl values from the batch items
//        var imageUrls = batch.batchItems
//            .Select(item => item.ImageUrl)
//            .Where(url => url != null)
//            .ToList();

//        return Ok(new ResponseModel
//        {
//            StatusCode = 200,
//            Data = imageUrls
//        });
//    }


//    // Create a new batch without product items
//    [HttpPost("create-batch")]
//    public IActionResult CreateBatch([FromBody] BatchCreateModel model)
//    {
//        if (!ModelState.IsValid)
//        {
//            return BadRequest(new ResponseModel
//            {
//                StatusCode = 400,
//                MessageError = "Invalid input data"
//            });
//        }

//        var batch = new Batch
//        {
//            name = model.Name,
//            Description = model.Description,
//            Quantity = 0, // Initial quantity is 0
//            Price = 0, // Initial price is 0
//            batchItems = new List<ProductItem>() // Initialize an empty list
//        };

//        _unitOfWork.BatchRepository.Create(batch);
//        _unitOfWork.SaveChange();

//        var response = new
//        {
//            BatchId = batch.Id,
//            BatchName = batch.name,
//            Description = batch.Description,
//            Quantity = batch.Quantity,
//            TotalPrice = batch.Price
//        };

//        return Ok(new ResponseModel
//        {
//            StatusCode = 200,
//            Data = response
//        });
//    }

//    // Add a product item to an existing batch
//    [HttpPost("add-productitem-to-batch")]
//    public IActionResult AddProductItemToBatch([FromBody] AddProductItemToBatchModel model)
//    {
//        if (!ModelState.IsValid)
//        {
//            return BadRequest(new ResponseModel
//            {
//                StatusCode = 400,
//                MessageError = "Invalid input data"
//            });
//        }

//        // Retrieve the specified product item
//        var productItem = _unitOfWork.ProductItemRepository.GetById(model.ProductItemId);

//        if (productItem == null || productItem.BatchId != null)
//        {
//            return BadRequest(new ResponseModel
//            {
//                StatusCode = 400,
//                MessageError = "Product item not found or already assigned to a batch"
//            });
//        }

//        // Retrieve the specified batch
//        var batch = _unitOfWork.BatchRepository.Get(b => b.Id == model.BatchId, includeProperties: b => b.batchItems).FirstOrDefault();

//        if (batch == null)
//        {
//            return NotFound(new ResponseModel
//            {
//                StatusCode = 404,
//                MessageError = "Batch not found"
//            });
//        }

//        // Check if the Batch's ProductId matches the ProductItem's ProductId
//        if (batch.ProductId == null)
//        {
//            // Set ProductId to match the ProductItem's ProductId if null
//            batch.ProductId = productItem.ProductId;
//        }
//        else if (batch.ProductId != productItem.ProductId)
//        {
//            return BadRequest(new ResponseModel
//            {
//                StatusCode = 400,
//                MessageError = "Product item belongs to a different product and cannot be added to this batch"
//            });
//        }

//        // Update batch properties
//        batch.Quantity += 1;
//        batch.Price += productItem.Price;
//        batch.batchItems.Add(productItem);

//        // Update product item with batch ID
//        productItem.BatchId = batch.Id;
//        _unitOfWork.ProductItemRepository.Update(productItem);
//        _unitOfWork.BatchRepository.Update(batch);
//        _unitOfWork.SaveChange();

//        var response = new
//        {
//            BatchId = batch.Id,
//            BatchName = batch.name,
//            Description = batch.Description,
//            Quantity = batch.Quantity,
//            TotalPrice = batch.Price,
//            ProductItems = batch.batchItems.Select(item => new
//            {
//                ProductItemId = item.Id,
//                Name = item.Name,
//                Price = item.Price,
//                ImageUrl = item.ImageUrl
//            }).ToList()
//        };

//        return Ok(new ResponseModel
//        {
//            StatusCode = 200,
//            Data = response
//        });
//    }

//    // Update batch
//    [HttpPut("update-batch/{id}")]
//    public IActionResult UpdateBatch(string id, [FromBody] BatchUpdateModel model)
//    {
//        var batch = _unitOfWork.BatchRepository.GetById(id);

//        if (batch == null)
//        {
//            return NotFound(new ResponseModel
//            {
//                StatusCode = 404,
//                MessageError = "Batch not found"
//            });
//        }

//        // Update only the name and description
//        batch.name = model.Name;
//        batch.Description = model.Description;
//        batch.LastUpdatedTime = DateTimeOffset.Now;

//        _unitOfWork.BatchRepository.Update(batch);

//        var response = new
//        {
//            BatchId = batch.Id,
//            BatchName = batch.name,
//            Description = batch.Description,
//            LastUpdatedTime = batch.LastUpdatedTime
//        };

//        return Ok(new ResponseModel
//        {
//            StatusCode = 200,
//            Data = response
//        });
//    }


//    // Remove ProductItem from Batch
//    [HttpDelete("remove-productitem-from-batch")]
//    public IActionResult RemoveProductItemFromBatch([FromBody] RemoveProductItemModel model)
//    {
//        // Validate input model
//        if (!ModelState.IsValid)
//        {
//            return BadRequest(new ResponseModel
//            {
//                StatusCode = 400,
//                MessageError = "Invalid input data"
//            });
//        }

//        // Retrieve the specified product item
//        var productItem = _unitOfWork.ProductItemRepository.GetById(model.ProductItemId);

//        if (productItem == null)
//        {
//            return NotFound(new ResponseModel
//            {
//                StatusCode = 404,
//                MessageError = "Product item not found"
//            });
//        }

//        // Check if the product item is associated with a batch
//        if (string.IsNullOrEmpty(productItem.BatchId))
//        {
//            return BadRequest(new ResponseModel
//            {
//                StatusCode = 400,
//                MessageError = "Product item is not part of any batch"
//            });
//        }

//        // Retrieve the batch associated with the product item
//        var batch = _unitOfWork.BatchRepository.Get(b => b.Id == productItem.BatchId, includeProperties: b => b.batchItems).FirstOrDefault();

//        if (batch == null)
//        {
//            return NotFound(new ResponseModel
//            {
//                StatusCode = 404,
//                MessageError = "Batch not found"
//            });
//        }

//        // Remove product item from the batch and set its BatchId to null
//        batch.batchItems.Remove(productItem);
//        productItem.BatchId = null;
//        _unitOfWork.ProductItemRepository.Update(productItem);

//        // If no items remain in the batch, delete the batch
//        if (!batch.batchItems.Any())
//        {
//            _unitOfWork.BatchRepository.Delete(batch);
//        }
//        else
//        {
//            // Update batch quantity and price if items still remain
//            batch.Quantity -= 1;
//            batch.Price -= productItem.Price;
//            _unitOfWork.BatchRepository.Update(batch);
//        }

//        _unitOfWork.SaveChange();

//        return Ok(new ResponseModel
//        {
//            StatusCode = 200,
//            Data = new { Message = "Product item removed from batch successfully" }
//        });
//    }

//}
