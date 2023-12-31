﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Models;
using PagedList;

namespace Model.Dao
{
    public class ProductDao
    {
        
        public ProductDao()
        {
            
        }
        public IEnumerable<Product> ListAllPaging(long cateID, string searchString, int page, int pagesize)
        {
            IQueryable<Product> model = DataProvider.Ins.DB.Products;
            if (cateID != -1)
            {
                model = model.Where(x => x.CategoryID == cateID);
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.MetaTitle.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pagesize);
        }
        public bool Delete(int id)
        {
            try
            {
                var product = DataProvider.Ins.DB.Products.Find(id);
                DataProvider.Ins.DB.Products.Remove(product);
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public long Insert(Product entity)
        {
            DataProvider.Ins.DB.Products.Add(entity);
            DataProvider.Ins.DB.SaveChanges();
            return entity.ID;
        }
        public Product ViewDetail(long id)
        {

            return DataProvider.Ins.DB.Products.Find(id);
        }
        public bool Update(Product entity)
        {
            try
            {
                var product = DataProvider.Ins.DB.Products.Find(entity.ID);
                product.Name = entity.Name;
                product.Code = entity.Code;
                product.MetaTitle = entity.MetaTitle;
                product.Description = entity.Description;
                product.Detail = entity.Detail;
                product.Image = entity.Image;
                product.ListType = entity.ListType;
                product.ListFile = entity.ListFile;
                product.CategoryID = entity.CategoryID;

                DataProvider.Ins.DB.SaveChanges();
                return true;

            }
            catch (Exception)
            {
                return false;

            }
        }
        public List<Product> ListAllProduct()
        {
            return DataProvider.Ins.DB.Products.Where(x => x.Status == true).OrderByDescending(x => x.ID).ToList();
        }
        public List<Product> ListByCategoryID(string searchString, long CategoryID, int page, int itemPerPage)
        {
            if (page < 1) page = 1;
            IOrderedQueryable<Product> model = DataProvider.Ins.DB.Products;
            if (CategoryID == 0)
            {
                if(!string.IsNullOrEmpty(searchString))
                {
                    return model.Where(x => x.Name.Contains(searchString) || x.Description.Contains(searchString)).Where(x => (bool)x.Status).OrderByDescending(x => x.CreateDate).Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();
                }
                else
                {
                    return model.Where(x=> (bool)x.Status).OrderByDescending(x => x.CreateDate).Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    return model.Where(x => x.Name.Contains(searchString) || x.Description.Contains(searchString)).Where(x => (bool)x.Status && x.CategoryID==CategoryID).OrderByDescending(x => x.CreateDate).Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();
                }
                else
                {
                    return model.Where(x => (bool)x.Status && x.CategoryID == CategoryID).OrderByDescending(x => x.CreateDate).Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();
                }
            }
          

        }

        public int CountByCategoryID(string searchString, long CategoryID)
        {
            IOrderedQueryable<Product> model = DataProvider.Ins.DB.Products;
            if (CategoryID == 0)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    return model.Where(x => x.Name.Contains(searchString) || x.Description.Contains(searchString)).Where(x => (bool)x.Status).Count();
                }
                else
                {
                    return model.Where(x => (bool)x.Status).Count();
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    return model.Where(x => x.Name.Contains(searchString) || x.Description.Contains(searchString)).Where(x => (bool)x.Status && x.CategoryID == CategoryID).Count();
                }
                else
                {
                    return model.Where(x => (bool)x.Status && x.CategoryID == CategoryID).Count();
                }
            }
        }

        public User GetCreatedByUser(int userId)
        {
            User user = DataProvider.Ins.DB.Users.Where(x => x.ID.Equals(userId)).FirstOrDefault();
            return user;
        }

        public int GetCountComment(long productId)
        {
            return DataProvider.Ins.DB.Comments.Where(x => x.ProductID == productId).ToList().Count();
        }

        public bool BuyProduct(int userId, int productId)
        {
            bool result = false;
            try
            {
                DataProvider.Ins.DB.OwnProducts.Where(x => x.idProduct == productId && x.idUser == userId).SingleOrDefault().IsBought = true;
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch 
            {
                return result;
            }
        }

        public bool AddProductToCart(int userId, int productId)
        {
            try
            {
                OwnProduct ownProduct = new OwnProduct() { idProduct = productId, idUser = userId, IsBought = false };
                DataProvider.Ins.DB.OwnProducts.Add(ownProduct);
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteProductFromCart(int userId, int productId)
        {
            try
            {
                OwnProduct ownProduct = DataProvider.Ins.DB.OwnProducts.Where(x =>  x.idProduct == productId && x.idUser == userId).SingleOrDefault();
                DataProvider.Ins.DB.OwnProducts.Remove(ownProduct);
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        

        public Dictionary<string, bool> GetWishListProduct(int userId)
        {
            Dictionary<string, bool> lisId = new Dictionary<string, bool>();
            foreach (var ownProduct in DataProvider.Ins.DB.OwnProducts.Where(x => x.idUser == userId))
            {
                if (ownProduct.IsBought == true)
                {
                    lisId.Add(ownProduct.idProduct.ToString(), true);
                }
                else
                {
                    lisId.Add(ownProduct.idProduct.ToString(), false);
                }
            }
            return lisId;
        }
    }
}
