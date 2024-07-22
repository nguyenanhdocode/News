using Application.Common;
using CloudinaryDotNet.Actions;
using Core.Entities;
using DataAccess.UnifOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.Services.Impl
{
    public class AssetService : IAssetService
    {
        private IUnitOfWork _uow;
        private IClaimService _claimService;

        public AssetService(IUnitOfWork unitOfWork
            , IClaimService claimService)
        {
            _uow = unitOfWork;
            _claimService = claimService;
        }

        public async Task<Guid> Upload(IFormFile file, string type)
        {
            try
            {
                using (var tran = new TransactionScope(TransactionScopeOption.Required
                    , new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }
                    , TransactionScopeAsyncFlowOption.Enabled))
                {
                    string fileId = Guid.NewGuid().ToString();
                    string fileName = string.Format("{0}{1}", fileId, Path.GetExtension(file.FileName));
                    string relativePath = string.Format("{0}/{1}", "uploads", fileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

                    var asset = new Asset
                    {
                        AssetId = fileId,
                        Path = relativePath,
                        Type = type,
                        CreatedUserId = _claimService.GetUserId(),
                    };

                    await _uow.AssetRepository.Add(asset);
                    await _uow.SaveChangesAsync();
                    
                    using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileSrteam);
                    }

                    tran.Complete();
                    return asset.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
