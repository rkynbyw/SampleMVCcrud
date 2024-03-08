using MyWebFormApp.BLL.DTOs;
using System.Collections.Generic;

namespace MyWebFormApp.BLL.Interfaces
{
    public interface IArticleBLL
    {
        void Insert(ArticleCreateDTO article);
        IEnumerable<ArticleDTO> GetArticleWithCategory();
        IEnumerable<ArticleDTO> GetArticleByCategory(int categoryId);
        int InsertWithIdentity(ArticleCreateDTO article);
        void Update(ArticleUpdateDTO article);
        void Delete(int id);
        ArticleDTO GetArticleById(int id);

        IEnumerable<ArticleDTO> GetWithPaging(int pageNumber, int pageSize, string name);

        int GetCountArticle(string name);
        int GetCountArticleByCategory(int categoryId, string search);
        IEnumerable<ArticleDTO> GetArticleByCategoryPaging(int categoryId, int pageNumber, int pageSize, string search);

    }
}
