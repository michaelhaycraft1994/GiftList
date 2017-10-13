using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftList.DTO;
using GiftList.Data;
using GiftList.Domain;

namespace GiftList.Service
{
    public class SearchService
    {
        public List<PersonDTO> persons(LoginDTO credentials)
        {
            List<Person> people;

            using (SearchDAL db = new SearchDAL(credentials.username, credentials.password))
            {
                people = db.getPeople();
            }


            return personDTOConversion(people);
        }

        private List<PersonDTO> personDTOConversion(List<Person> people)
        {
            List<PersonDTO> peopleDTO = new List<PersonDTO>();

            foreach (Person person in people)
            {
                peopleDTO.Add(new PersonDTO()
                {
                    personID = person.personID,
                    personName = person.personName
                });
            }

            return peopleDTO;
        }

        public List<CategoryDTO> categories(LoginDTO credentials)
        {
            List<Category> categories;

            using (SearchDAL db = new SearchDAL(credentials.username, credentials.password))
            {
                categories = db.getCategories();
            }

            return categoryDTOConversion(categories);
        }

        private List<CategoryDTO> categoryDTOConversion(List<Category> things)
        {
            List<CategoryDTO> categoriesDTO = new List<CategoryDTO>();

            foreach (Category category in things)
            {
                categoriesDTO.Add(new CategoryDTO()
                {
                    categoryID = category.pk_CategoryID,
                    categoryName = category.CategoryName
                });
            }

            return categoriesDTO;
        }

        public List<ResultsDTO> getResults(LoginDTO credentials, SearchCriteriaDTO criteria)
        {
            List<Item> items;

            using (SearchDAL db = new SearchDAL(credentials.username, credentials.password))
            {
                items = db.getItems(criteria);
            }

            return resultsDTOConversion(items);
        }

        private List<ResultsDTO> resultsDTOConversion(List<Item> items)
        {
            List<ResultsDTO> results = new List<ResultsDTO>();

            foreach (Item item in items)
            {
                results.Add(new ResultsDTO()
                {
                    itemName = item.itemName,
                    ItemPrice = item.itemPrice,
                    description = item.description
                });
            }

            return results;
        }

        public void addItem(LoginDTO credentials, ItemDTO items)
        {
            using (SearchDAL db = new SearchDAL(credentials.username, credentials.password))
            {
                db.addItem(ItemDTOconversion(items));
            }
        }

        private Item ItemDTOconversion(ItemDTO items)
        {
            Item itemToAdd = new Item();

            itemToAdd.fk_personID = items.personID;
            itemToAdd.fk_CategoryID = items.categoryID;
            itemToAdd.description = items.description;
            itemToAdd.itemName = items.itemName;
            itemToAdd.itemPrice = items.itemPrice.Value;

            return itemToAdd;
        }

        public void deleteItem(LoginDTO credentials, ItemDTO items)
        {
            using (SearchDAL db = new SearchDAL(credentials.username, credentials.password))
            {
                db.deleteItem(deleteItemDTOconversion(items));
            }
        }

        private Item deleteItemDTOconversion(ItemDTO items)
        {
            Item itemToAdd = new Item();

            itemToAdd.fk_personID = items.personID;
            itemToAdd.fk_CategoryID = items.categoryID;
            itemToAdd.itemName = items.itemName;

            return itemToAdd;
        }
    }
}
