using Lecture_9_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_9_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_9_Tests
{
    [TemplatedTestClass]
    public class Exercise_3_Tests_Template
    {
        #region Exercise 3A
        [TestMethod("a. Product.ID is a public property"), TestCategory("Exercise 3A")]
        public void ProductIDISAPublicProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Product, int>(s => s.ID);
            test.Execute();
        }

        [TestMethod("b. Product.Name is a public property"), TestCategory("Exercise 3A")]
        public void ProductTitleIsAPublicProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Product, string>(s => s.Name);
            test.Execute();
        }

        [TestMethod("c. Product.Category is a public property"), TestCategory("Exercise 3A")]
        public void ProductCategoryIsAPublicProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Product, string>(s => s.Category);
            test.Execute();
        }

        [TestMethod("d. Product.Price is a public property"), TestCategory("Exercise 3A")]
        public void StudentAgeIsAPublicProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Product, decimal>(s => s.Price);
            test.Execute();
        }

        [TemplatedTestMethod("e. Product.Equals equates products with same ID"), TestCategory("Exercise 3A")]
        public void ProductEqualsEquatesProductsWithSameID()
        {
            Product product1 = new Product() { ID = 5 };
            Product product2 = new Product() { ID = 5 };

            Assert.IsTrue(product1.Equals(product2));
        }

        [TemplatedTestMethod("f. Product.Equals does not equate products with different ID"), TestCategory("Exercise 3A")]
        public void ProductEqualsDoesNotEquateProductsWithDifferentID()
        {
            Product product1 = new Product() { ID = 4 };
            Product product2 = new Product() { ID = 5 };

            Assert.IsFalse(product1.Equals(product2));
        }

        [TemplatedTestMethod("g. Product.GetHashCode equates products with same ID"), TestCategory("Exercise 3A")]
        public void ProductGetHashCodeEquatesProductsWithSameID()
        {
            Product product1 = new Product() { ID = 5 };
            Product product2 = new Product() { ID = 5 };

            Assert.IsTrue(product1.GetHashCode() == product2.GetHashCode());
        }

        [TemplatedTestMethod("h. Product.GetHashCode does not equate products with different ID"), TestCategory("Exercise 3A")]
        public void ProductGetHashCodeDoesNotEquateProductsWithDifferentID()
        {
            Product product1 = new Product() { ID = 4 };
            Product product2 = new Product() { ID = 5 };

            Assert.IsFalse(product1.GetHashCode() == product2.GetHashCode());
        }
        #endregion

        #region Exercise 3B
        [TestMethod("a. ProductRepository implements IEnumerable<T>"), TestCategory("Exercise 3B")]
        public void ProductRepositoryImplementsIEnumerable()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertClass<ProductRepository>(new TypeIsSubclassOfVerifier(typeof(IEnumerable<Product>)));
            test.Execute();
        }
        #endregion

        #region Exercise 3C
        [TestMethod("a. ProductRepository.Add(Product p) is a public method"), TestCategory("Exercise 3C")]
        public void ProductRepositoryAddIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<ProductRepository, Product>((r, p) => r.Add(p));
            test.Execute();
        }

        [TestMethod("b. ProductRepository.Update(Product p) is a public method"), TestCategory("Exercise 3C")]
        public void ProductRepositoryUpdateIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<ProductRepository, Product>((r, p) => r.Update(p));
            test.Execute();
        }

        [TestMethod("c. ProductRepository.Delete(Product p) is a public method"), TestCategory("Exercise 3C")]
        public void ProductRepositoryDeleteIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<ProductRepository, Product>((r, p) => r.Update(p));
            test.Execute();
        }

        [TemplatedTestMethod("d. ProductRepository.Add(Product p) adds product"), TestCategory("Exercise 3C")]
        public void ProductRepositoryAddAddsProduct()
        {
            ProductRepository repository = new ProductRepository();
            Product product = new Product();

            repository.Add(product);

            Assert.IsTrue(repository.SequenceEqual(new Product[] { product }));
        }

        [TemplatedTestMethod("e. ProductRepository.Add(Product p) adds product, which cannot be affected"), TestCategory("Exercise 3C")]
        public void ProductRepositoryAddAddsProduct2()
        {
            Product product = new Product() { Name = "Name" };
            ProductRepository repository = new ProductRepository() { product };

            product.Name = "NewName";

            Assert.AreEqual("Name", repository.First().Name);
        }

        [TemplatedTestMethod("f. ProductRepository.Update(Product p) updates product, which cannot be affected"), TestCategory("Exercise 3C")]
        public void ProductRepositoryUpdatesProduct()
        {
            Product product = new Product() { Name = "Name" };
            ProductRepository repository = new ProductRepository() { product };

            product.Name = "NewName";
            repository.Update(product);

            Assert.AreEqual("NewName", repository.First().Name);
        }

        [TemplatedTestMethod("g. ProductRepository.Delete(Product p) removes product again"), TestCategory("Exercise 3C")]
        public void ProductRepositoryDeleteRemovesProductAgain()
        {
            ProductRepository repository = new ProductRepository();
            Product product = new Product();

            repository.Add(product);
            repository.Delete(product);

            Assert.IsFalse(repository.Any());
        }
        #endregion

        #region Exercise 3D
        [TestMethod("a. ProductRepository.GetProductByID(int id) is a public method"), TestCategory("Exercise 3D")]
        public void ProductRepositoryGetProductByIDIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<ProductRepository, int, Product>((r, i) => r.GetProductByID(i));
            test.Execute();
        }

        [TestMethod("b. ProductRepository.GetLeastExpensiveProduct() is a public method"), TestCategory("Exercise 3D")]
        public void ProductRepositoryGetLeastExpensiveProductIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<ProductRepository, Product>(r => r.GetLeastExpensiveProduct());
            test.Execute();
        }

        [TestMethod("c. ProductRepository.GetMostExpensiveProduct() is a public method"), TestCategory("Exercise 3D")]
        public void ProductRepositoryGetMostExpensiveProductIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<ProductRepository, Product>(r => r.GetMostExpensiveProduct());
            test.Execute();
        }

        [TestMethod("d. ProductRepository.GetAverageProductPrice() is a public method"), TestCategory("Exercise 3D")]
        public void ProductRepositoryGetAverageProductPriceIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<ProductRepository, decimal>(r => r.GetAverageProductPrice());
            test.Execute();
        }

        [TestMethod("e. ProductRepository.GetProductsInCategory(string category) is a public method"), TestCategory("Exercise 3D")]
        public void ProductRepositoryGetProductsInCategoryIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<ProductRepository, string, IEnumerable<Product>>((r, s) => r.GetProductsInCategory(s));
            test.Execute();
        }

        [TestMethod("f. ProductRepository.GetProductCategories() is a public method"), TestCategory("Exercise 3D")]
        public void ProductRepositoryGetProductCategoriesIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<ProductRepository, IEnumerable<string>>(r => r.GetProductCategories());
            test.Execute();
        }

        [TemplatedTestMethod("g. ProductRepository.GetProductByID(int id) returns correctly"), TestCategory("Exercise 3D")]
        public void ProductRepositoryGetProductByIDReturnsCorrectly()
        {
            Product product = new Product() { ID = 5 };
            ProductRepository repository = new ProductRepository() { product };

            Assert.AreEqual(product, repository.GetProductByID(5));
        }

        [TemplatedTestMethod("h. ProductRepository.GetLeastExpensiveProduct() returns correctly"), TestCategory("Exercise 3D")]
        public void ProductRepositoryGetLeastExpensiveProductReturnsCorrectly()
        {
            Product leastExpensiveProduct = new Product() { ID = 4, Price = 10M };
            Product mostExpensiveProduct = new Product() { ID = 5, Price = 20M };
            ProductRepository repository = new ProductRepository()
            {
                leastExpensiveProduct,
                mostExpensiveProduct
            };

            Assert.AreEqual(leastExpensiveProduct, repository.GetLeastExpensiveProduct());
        }

        [TemplatedTestMethod("i. ProductRepository.GetMostExpensiveProduct() returns correctly"), TestCategory("Exercise 3D")]
        public void ProductRepositoryGetMostExpensiveProductReturnsCorrectly()
        {
            Product leastExpensiveProduct = new Product() { ID = 4, Price = 10M };
            Product mostExpensiveProduct = new Product() { ID = 5, Price = 20M };
            ProductRepository repository = new ProductRepository()
            {
                leastExpensiveProduct,
                mostExpensiveProduct
            };

            Assert.AreEqual(mostExpensiveProduct, repository.GetMostExpensiveProduct());
        }

        [TemplatedTestMethod("i. ProductRepository.GetAverageProductPrice() returns correctly"), TestCategory("Exercise 3D")]
        public void ProductRepositoryGetAverageProductPriceReturnsCorrectly()
        {
            Product leastExpensiveProduct = new Product() { ID = 4, Price = 10M };
            Product mostExpensiveProduct = new Product() { ID = 5, Price = 20M };
            ProductRepository repository = new ProductRepository()
            {
                leastExpensiveProduct,
                mostExpensiveProduct
            };

            Assert.AreEqual(15M, repository.GetAverageProductPrice());
        }

        [TemplatedTestMethod("j. ProductRepository.GetProductsInCategory(string category) returns correctly"), TestCategory("Exercise 3D")]
        public void ProductRepositoryGetProductsInCategoryReturnsCorrectly()
        {
            Product product1 = new Product() { ID = 4, Category = "Food" };
            Product product2 = new Product() { ID = 5, Category = "Food" };
            Product product3 = new Product() { ID = 6, Category = "Electronics" };
            ProductRepository repository = new ProductRepository()
            {
                product1,
                product2,
                product3
            };

            Assert.IsTrue(repository.GetProductsInCategory("Electronics").SequenceEqual(new Product[] { product3 }));
        }

        [TemplatedTestMethod("k. ProductRepository.GetProductCategories() returns correctly"), TestCategory("Exercise 3D")]
        public void ProductRepositoryGetProductCategoriesReturnsCorrectly()
        {
            Product product1 = new Product() { ID = 4, Category = "Food" };
            Product product2 = new Product() { ID = 5, Category = "Electronics" };
            ProductRepository repository = new ProductRepository()
            {
                product1,
                product2
            };

            Assert.IsTrue(repository.GetProductCategories().SequenceEqual(new string[] { "Food", "Electronics" }));
        }
        #endregion    
    }
}
