﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Model.Product;
using Repository.Model;
using Repository.Repository;
using Repository.Model.Promotion;
using Repository.Data.Entity;
using Microsoft.AspNetCore.Authorization;

namespace koi_farm_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PromotionController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("get-all-promotion")]
        public IActionResult GetAllPromotions()
        {
            var promotions = _unitOfWork.PromotionRepository.GetAll();

            if (!promotions.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "No promotions found."
                });
            }

            var responsePromotions = _mapper.Map<List<ResponsePromotionModel>>(promotions);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responsePromotions
            });
        }

        [HttpGet("get-promotion-by-code/{code}")]
        public IActionResult GetPromotionByCode(string code)
        {
            var promotion = _unitOfWork.PromotionRepository.GetAll().Where(p => p.Code == code);

            if (!promotion.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "No promotions found."
                });
            }

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = promotion
            });
        }

        [HttpGet("get-promotion/{id}")]
        public IActionResult GetPromotion(string id)
        {
            var promotion = _unitOfWork.PromotionRepository.GetById(id);

            if (promotion == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Promotion not found."
                });
            }

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = promotion
            });
        }

        [Authorize(Roles = "Manager,Staff")]
        [HttpPost("create-promotion")]
        public IActionResult CreatePromotion(RequestCreatePromotionModel promotionModel)
        {
            if (promotionModel == null || string.IsNullOrEmpty(promotionModel.Code) || promotionModel.Amount <= 0 || string.IsNullOrEmpty(promotionModel.Type))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid promotion data. Code, Amount and Type are required."
                });
            }

            var existingPromotion = _unitOfWork.PromotionRepository.GetSingle(p => p.Code.ToLower() == promotionModel.Code.ToLower());
            if (existingPromotion != null)
            {
                return Conflict(new ResponseModel
                {
                    StatusCode = 409,
                    MessageError = "Code already exists."
                });
            }

            if (promotionModel.Type != "Direct" && promotionModel.Type != "Percentage")
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid promotion type. It should only be Direct or Percentage."
                });
            }

            if (promotionModel.Type == "Percentage" && promotionModel.Amount > 100)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid promotion amount. It should be lower than 100 if the type is Percentage"
                });
            }

            var promotion = _mapper.Map<Promotion>(promotionModel);

            _unitOfWork.PromotionRepository.Create(promotion);

            return Ok(new ResponseModel
            {
                StatusCode = 201,
                Data = promotion
            });
        }

        [Authorize(Roles = "Manager,Staff")]
        [HttpPut("update-promotion/{id}")]
        public IActionResult UpdatePromotion(string id, [FromBody] RequestCreatePromotionModel promotionModel)
        {
            if (string.IsNullOrEmpty(id) || promotionModel == null || string.IsNullOrEmpty(promotionModel.Code) ||
                promotionModel.Amount < 0 || string.IsNullOrEmpty(promotionModel.Type))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid promotion data. ID and promotion data are required."
                });
            }

            var promotion = _unitOfWork.PromotionRepository.GetById(id);
            if (promotion == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Promotion not found."
                });
            }

            var existingPromotion = _unitOfWork.PromotionRepository.GetSingle(p => p.Code.ToLower() == promotionModel.Code.ToLower() && p.Id != id);
            if (existingPromotion != null)
            {
                return Conflict(new ResponseModel
                {
                    StatusCode = 409,
                    MessageError = "Promotion code already exists."
                });
            }

            if (promotionModel.Type != "Direct" && promotionModel.Type != "Percentage")
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid promotion type. It should only be Direct or Percentage."
                });
            }

            if (promotionModel.Type == "Percentage" && promotionModel.Amount > 100)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid promotion amount. It should be lower than 100 if the type is Percentage"
                });
            }

            _mapper.Map(promotionModel, promotion);

            _unitOfWork.PromotionRepository.Update(promotion);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = promotion
            });
        }

        [Authorize(Roles = "Manager,Staff")]
        [HttpDelete("delete-promotion/{id}")]
        public IActionResult DeletePromotion(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "PromotionId cannot be null or empty."
                });
            }

            var promotion = _unitOfWork.PromotionRepository.GetById(id);

            if (promotion == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = $"Promotion with Id: {id} not found."
                });
            }

            _unitOfWork.PromotionRepository.Delete(promotion);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = $"Promotion with ID {id} successfully deleted."
            });
        }
    }
}
