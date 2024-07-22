using Core.Common;
using Core.Entities;
using DataAccess.Persistence;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace DataAccess.UnifOfWork.Impl
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext DbContext { get; }

        public IBaseRepository<Asset> AssetRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }
        public IPostRepository PostRepository { get; set; }
        public ITwoFactorAuthRepository TfaRepository { get; set; }

        public UnitOfWork(AppDbContext dbContext
            , IBaseRepository<Asset> assetRepo
            , ICategoryRepository cateRepo
            , IPostRepository postRepo
            , IHttpContextAccessor httpContextAccessor
            , ITwoFactorAuthRepository tfaRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            DbContext = dbContext;

            AssetRepository = assetRepo;
            AssetRepository.Dbcontext = DbContext;

            CategoryRepository = cateRepo;
            CategoryRepository.Dbcontext = DbContext;

            PostRepository = postRepo;
            PostRepository.Dbcontext = DbContext;
            
            TfaRepository = tfaRepository;
            TfaRepository.Dbcontext = DbContext;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }
    }
}
