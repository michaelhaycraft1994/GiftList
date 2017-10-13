using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using GiftList.Domain;
using GiftList.DTO;

namespace GiftList.Data
{
    public class SearchDAL : IDisposable
    {
        private SqlConnection conn;

        public SearchDAL(String userName, String password)
        {
            String connectionString = "Server=71.9.179.69;Address = 71.9.179.69,1433;Database=Gifts;Uid=" + userName + ";Pwd=" + password;
            conn = new SqlConnection(connectionString);
            conn.Open();
        }

        public List<Person> getPeople()
        {
            String queryString = @"SELECT pk_PersonID, PersonName FROM Person ORDER BY pk_PersonID";

            SqlCommand userQuery = new SqlCommand(queryString, conn);

            return readPeople(userQuery);
        }

        private List<Person> readPeople(SqlCommand userQuery)
        {
            List<Person> people = new List<Person>();

            try
            {
                SqlDataReader rdr = userQuery.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(rdr);
                foreach (DataRow row in dataTable.Rows)
                {
                    Person individual = new Person();
                    individual.personID = row.Field<int>("pk_PersonID");
                    individual.personName = row.Field<String>("PersonName");
                    people.Add(individual);
                }

                rdr.Close();
            }
            catch
            {
                return null;
            }
            return people;
        }

        public List<Category> getCategories()
        {
            String queryString = @"SELECT pk_CategoryID, CategoryName FROM Categories ORDER BY pk_CategoryID";

            SqlCommand userQuery = new SqlCommand(queryString, conn);

            return readCategory(userQuery);
        }

        private List<Category> readCategory(SqlCommand userQuery)
        {
            List<Category> categories = new List<Category>();

            try
            {
                SqlDataReader rdr = userQuery.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(rdr);
                foreach (DataRow row in dataTable.Rows)
                {
                    Category category = new Category();
                    category.pk_CategoryID = row.Field<int>("pk_CategoryID");
                    category.CategoryName = row.Field<String>("CategoryName");
                    categories.Add(category);
                }

                rdr.Close();
            }
            catch
            {
                return null;
            }

            return categories;
        }

        public List<Item> getItems(SearchCriteriaDTO criteria)
        {
            String queryString;

            if (criteria.sortZ == true)
            {
                if (criteria.categoryID == 0)
                {
                    queryString = @"SELECT ItemName, ItemPrice, Description FROM Item
                                WHERE fk_personID = @personKey AND ItemPrice < @maxPrice
                                ORDER BY ItemName DESC";
                }
                else
                {
                    queryString = @"SELECT ItemName, ItemPrice, Description FROM Item
                                WHERE fk_CategoryID = @categoryKey AND fk_personID = @personKey AND ItemPrice < @maxPrice
                                ORDER BY ItemName DESC";
                }
            }
            else if (criteria.sortHighValue == true)
            {
                if (criteria.categoryID == 0)
                {
                    queryString = @"SELECT ItemName, ItemPrice, Description FROM Item
                                WHERE fk_personID = @personKey AND ItemPrice < @maxPrice
                                ORDER BY ItemPrice DESC";
                }
                else
                {
                    queryString = @"SELECT ItemName, ItemPrice, Description FROM Item
                                WHERE fk_CategoryID = @categoryKey AND fk_personID = @personKey AND ItemPrice < @maxPrice
                                ORDER BY ItemPrice DESC";
                }
            }
            else if (criteria.sortLowValue == true)
            {
                if (criteria.categoryID == 0)
                {
                    queryString = @"SELECT ItemName, ItemPrice, Description FROM Item
                                WHERE fk_personID = @personKey AND ItemPrice < @maxPrice
                                ORDER BY ItemPrice ASC";
                }
                else
                {
                    queryString = @"SELECT ItemName, ItemPrice, Description FROM Item
                                WHERE fk_CategoryID = @categoryKey AND fk_personID = @personKey AND ItemPrice < @maxPrice
                                ORDER BY ItemPrice ASC";
                }
            }
            else
            {
                if (criteria.categoryID == 0)
                {
                    queryString = @"SELECT ItemName, ItemPrice, Description FROM Item
                                WHERE fk_personID = @personKey AND ItemPrice < @maxPrice
                                ORDER BY ItemName ASC";
                }
                else
                {
                    queryString = @"SELECT ItemName, ItemPrice, Description FROM Item
                                WHERE fk_CategoryID = @categoryKey AND fk_personID = @personKey AND ItemPrice < @maxPrice
                                ORDER BY ItemName ASC";
                }
            }

            SqlCommand userQuery = new SqlCommand(queryString, conn);

            userQuery.Parameters.Add(new SqlParameter("@categoryKey", SqlDbType.Int));
            userQuery.Parameters.Add(new SqlParameter("@personKey", SqlDbType.Int));
            userQuery.Parameters.Add(new SqlParameter("@maxPrice", SqlDbType.Decimal));

            userQuery.Parameters["@categoryKey"].Value = criteria.categoryID;
            userQuery.Parameters["@personKey"].Value = criteria.personID;
            userQuery.Parameters["@maxPrice"].Value = criteria.maxMoney;


            return readItems(userQuery);
        }

        private List<Item> readItems(SqlCommand userQuery)
        {
            List<Item> items = new List<Item>();

            try
            {
                SqlDataReader rdr = userQuery.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(rdr);
                foreach (DataRow row in dataTable.Rows)
                {
                    Item item = new Item();
                    item.itemName = row.Field<String>("ItemName");
                    item.itemPrice = row.Field<Decimal>("ItemPrice");
                    item.description = row.Field<String>("Description");
                    items.Add(item);
                }

                rdr.Close();
            }
            catch
            {
                return null;
            }

            return items;
        }

        public void addItem(Item itemToAdd)
        {
            String queryString = @"INSERT INTO Item (fk_personID, fk_CategoryID, ItemName, Description, ItemPrice)
                                    VALUES (@personKey, @categoryKey, @itemName, @description, @itemPrice)";

            SqlCommand userQuery = new SqlCommand(queryString, conn);

            userQuery.Parameters.Add(new SqlParameter("@personKey", SqlDbType.Int));
            userQuery.Parameters.Add(new SqlParameter("@categoryKey", SqlDbType.Int));
            userQuery.Parameters.Add(new SqlParameter("@itemName", SqlDbType.NVarChar));
            userQuery.Parameters.Add(new SqlParameter("@description", SqlDbType.NVarChar));
            userQuery.Parameters.Add(new SqlParameter("@itemPrice", SqlDbType.Decimal));

            userQuery.Parameters["@personKey"].Value = itemToAdd.fk_personID;
            userQuery.Parameters["@categoryKey"].Value = itemToAdd.fk_CategoryID;
            userQuery.Parameters["@itemName"].Value = itemToAdd.itemName;
            userQuery.Parameters["@description"].Value = itemToAdd.description;
            userQuery.Parameters["@itemPrice"].Value = itemToAdd.itemPrice;

            userQuery.ExecuteNonQuery();
        }

        public void deleteItem(Item itemToDelete)
        {
            String queryString = @"DELETE FROM Item
                                 WHERE fk_personID = @personKey AND ItemName = @itemName";

            SqlCommand userQuery = new SqlCommand(queryString, conn);

            userQuery.Parameters.Add(new SqlParameter("@personKey", SqlDbType.Int));
            userQuery.Parameters.Add(new SqlParameter("@itemName", SqlDbType.NVarChar));

            userQuery.Parameters["@personKey"].Value = itemToDelete.fk_personID;
            userQuery.Parameters["@itemName"].Value = itemToDelete.itemName;

            userQuery.ExecuteNonQuery();
        }

        public void Dispose()
        {
            if (conn != null)
            {
                try
                {
                    conn.Close();
                }
                catch
                {

                }
            }
        }
    }
}