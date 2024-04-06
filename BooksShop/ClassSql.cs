using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShop
{
    class ClassSql
    {
        public static string GetConnSQL()
        {
            return @"Data Source = DESKTOP-MJM6IQP\SQLEXPRESS;Initial Catalog=BookShop;Integrated Security=True";
        }
    }
}
