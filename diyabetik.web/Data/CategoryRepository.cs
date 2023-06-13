using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diyabetik.model;

namespace diyabetik.web.Data
{
    public class CategoryRepository
    {   

        // oluşturlan kategoriler, kategoriler veritabanında da bulundurulabilirdi
        // farklı kullanım şekillerini öğrenmek için bnu şekilde yapıldı
        private static List<Category> _categories = null;
        static CategoryRepository()
        {
            _categories = new List<Category>
            {
                new Category {Id = "1", Name = "Kan Şekeri Verileri", Description= "Örnek Açıklama"},
                new Category {Id = "2", Name = "İlaç Kullanım Verileri", Description= "Örnek Açıklama"},
                new Category {Id = "3", Name = "Tansiyon Verileri", Description= "Örnek Açıklama"},
                new Category {Id = "4", Name = "Kişisel Bilgiler", Description= "Örnek Açıklama"},
                new Category {Id = "5", Name = "Adımsayar Verileri", Description= "Örnek Açıklama"},
                new Category {Id = "6", Name = "Tedavi Verileri", Description= "Örnek Açıklama"}
            };
        }
        public static List<Category> Categories
        {
            get{        
                return _categories;
            }
        }
        public static void AddCategory(Category category)
        {
            _categories.Add(category);
        }
        
    }
}