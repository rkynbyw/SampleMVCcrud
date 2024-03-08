﻿using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;
using MyWebFormApp.BO;
using MyWebFormApp.DAL;
using MyWebFormApp.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace MyWebFormApp.BLL
{
    public class ArticleBLL : IArticleBLL
    {
        private readonly IArticleDAL _articleDAL;
        public ArticleBLL()
        {
            _articleDAL = new ArticleDAL();
        }

        public void Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ArticleID is required");
            }

            try
            {
                _articleDAL.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<ArticleDTO> GetArticleByCategory(int categoryId)
        {
            List<ArticleDTO> articles = new List<ArticleDTO>();
            var articlesFromDAL = _articleDAL.GetArticleByCategory(categoryId);
            foreach (var article in articlesFromDAL)
            {
                articles.Add(new ArticleDTO
                {
                    ArticleID = article.ArticleID,
                    CategoryID = article.CategoryID,
                    Title = article.Title,
                    Details = article.Details,
                    PublishDate = article.PublishDate,
                    IsApproved = article.IsApproved,
                    Pic = article.Pic,
                    Category = new CategoryDTO
                    {
                        CategoryID = article.Category.CategoryID,
                        CategoryName = article.Category.CategoryName
                    }
                });
            }
            return articles;
        }

        public ArticleDTO GetArticleById(int id)
        {
            ArticleDTO article = new ArticleDTO();
            var articleFromDAL = _articleDAL.GetById(id);

            if (articleFromDAL == null)
            {
                throw new ArgumentException($"Data article with id:{id} not found");
            }

            article.ArticleID = articleFromDAL.ArticleID;
            article.CategoryID = articleFromDAL.CategoryID;
            article.Title = articleFromDAL.Title;
            article.Details = articleFromDAL.Details;
            article.PublishDate = articleFromDAL.PublishDate;
            article.IsApproved = articleFromDAL.IsApproved;
            article.Pic = articleFromDAL.Pic;

            return article;
        }

        public IEnumerable<ArticleDTO> GetArticleWithCategory()
        {
            List<ArticleDTO> articles = new List<ArticleDTO>();
            var articlesFromDAL = _articleDAL.GetArticleWithCategory();
            foreach (var article in articlesFromDAL)
            {
                articles.Add(new ArticleDTO
                {
                    ArticleID = article.ArticleID,
                    CategoryID = article.CategoryID,
                    Title = article.Title,
                    Details = article.Details,
                    PublishDate = article.PublishDate,
                    IsApproved = article.IsApproved,
                    Pic = article.Pic,
                    Category = new CategoryDTO
                    {
                        CategoryID = article.Category.CategoryID,
                        CategoryName = article.Category.CategoryName
                    }
                });
            }
            return articles;
        }

        public int GetCountArticle(string name)
        {
            return _articleDAL.GetCountArticle(name);
        }





        public void Insert(ArticleCreateDTO articleDto)
        {
            if (string.IsNullOrEmpty(articleDto.Title))
            {
                throw new ArgumentException("Title is required");
            }
            if (string.IsNullOrEmpty(articleDto.Details))
            {
                throw new ArgumentException("Details is required");
            }

            try
            {
                var article = new Article
                {
                    CategoryID = articleDto.CategoryID,
                    Title = articleDto.Title,
                    Details = articleDto.Details,
                    IsApproved = articleDto.IsApproved,
                    Pic = articleDto.Pic
                };
                _articleDAL.Insert(article);
            }
            catch (System.Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public int InsertWithIdentity(ArticleCreateDTO articleDto)
        {
            if (string.IsNullOrEmpty(articleDto.Title))
            {
                throw new ArgumentException("Title is required");
            }
            if (string.IsNullOrEmpty(articleDto.Details))
            {
                throw new ArgumentException("Details is required");
            }

            try
            {
                var article = new Article
                {
                    CategoryID = articleDto.CategoryID,
                    Title = articleDto.Title,
                    Details = articleDto.Details,
                    IsApproved = articleDto.IsApproved,
                    Pic = articleDto.Pic
                };
                int result = _articleDAL.InsertWithIdentity(article);
                return result;
            }
            catch (System.Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public void Update(ArticleUpdateDTO article)
        {
            if (article.ArticleID <= 0)
            {
                throw new ArgumentException("ArticleID is required");
            }
            if (string.IsNullOrEmpty(article.Title))
            {
                throw new ArgumentException("Title is required");
            }
            if (string.IsNullOrEmpty(article.Details))
            {
                throw new ArgumentException("Details is required");
            }

            try
            {
                var updateArticle = new Article
                {
                    ArticleID = article.ArticleID,
                    CategoryID = article.CategoryID,
                    Title = article.Title,
                    Details = article.Details,
                    IsApproved = article.IsApproved,
                    Pic = article.Pic
                };

                _articleDAL.Update(updateArticle);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }


        public int GetCountArticleByCategory(int categoryId, string search)
        {
            try
            {
                return _articleDAL.GetCountArticleByCategory(categoryId, search);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<ArticleDTO> GetWithPaging(int pageNumber, int pageSize, string name)
        {
            List<ArticleDTO> listArticleDTO = new List<ArticleDTO>();
            var articles = _articleDAL.GetWithPaging(pageNumber, pageSize, name);
            foreach (var article in articles)
            {
                listArticleDTO.Add(new ArticleDTO
                {
                    ArticleID = article.ArticleID,
                    Title = article.Title,
                    Details = article.Details,
                    Pic = article.Pic,
                    PublishDate = article.PublishDate,
                    CategoryID = article.CategoryID,
                    CategoryName = article.Category.CategoryName,
                });
            }
            return listArticleDTO;
        }

        public IEnumerable<ArticleDTO> GetArticleByCategoryPaging(int categoryId, int pageNumber, int pageSize, string search)
        {
            List<ArticleDTO> listArticleDTO = new List<ArticleDTO>();
            try
            {
                var articles = _articleDAL.GetArticleByCategoryPaging(categoryId, pageNumber, pageSize, search);

                foreach (var article in articles)
                {
                    listArticleDTO.Add(new ArticleDTO
                    {
                        ArticleID = article.ArticleID,
                        Title = article.Title,
                        Details = article.Details,
                        Pic = article.Pic,
                        PublishDate = article.PublishDate,
                        CategoryID = article.CategoryID,
                        CategoryName = article.Category.CategoryName,
                        // Category = new CategoryDTO
                        // {
                        //     CategoryID = article.Category.CategoryID,
                        //     CategoryName = article.Category.CategoryName
                        // },

                    });
                }
                return listArticleDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
