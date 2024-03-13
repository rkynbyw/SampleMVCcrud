using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyWebFormApp.BLL.DTOs;

namespace SampleMVC.ViewModels

{
    public class ArticlesByCategoryViewModel
    {
        public int CategoryID { get; set; }
        public SelectList Categories { get; set; }
        public List<ArticleDTO> Articles { get; set; }
    }
}