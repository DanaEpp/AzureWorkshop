using System;
using System.Collections.Generic;

namespace AzureWorkshop.Models
{
    public static class People
    {
        public static List<Person> GetPeople()
        {
            var p = new List<Person>()
            {
                new Person() { Id=1, FirstName="Fred", LastName="Blazek",
                    StartDate = new DateTime(1975, 10, 17), Rating=6 },
                new Person() { Id=2, FirstName="Dylan", LastName="Hunt", 
                    StartDate = new DateTime(2000, 10, 2), Rating=8 },
                new Person() { Id=3, FirstName="John", LastName="Montana", 
                    StartDate = new DateTime(1989, 3, 19), Rating=7 },
                new Person() { Id=4, FirstName="Dave", LastName="Sheridan", 
                    StartDate = new DateTime(1978, 2, 15), Rating=9 },
                new Person() { Id=5, FirstName="Jason", LastName="Livenstone", 
                    StartDate = new DateTime(1994, 1, 26), Rating=6 },
                new Person() { Id=6, FirstName="Dante", LastName="Jones", 
                    StartDate = new DateTime(2001, 11, 1), Rating=5 },
                new Person() { Id=7, FirstName="Isaac", LastName="Sinclair", 
                    StartDate = new DateTime(1974, 9, 10), Rating=4 }
            };
            
            return p;
        }
    }

}