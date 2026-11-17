using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using Practical_8.Models;

namespace Practical_8.Controllers
{
    public class ProductController : Controller
    {
        private readonly string cs =
            System.Configuration.ConfigurationManager.ConnectionStrings["InventoryDB"].ConnectionString;

        // GET: Product
        public ActionResult Index(string searchString)
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ProductId, ProductName, Quantity, Price FROM Products";

                if (!String.IsNullOrWhiteSpace(searchString))
                {
                    query += " WHERE ProductName LIKE @SearchString";
                }

                query += " ORDER BY ProductId";

                SqlCommand cmd = new SqlCommand(query, con);
                if (!String.IsNullOrWhiteSpace(searchString))
                {
                    cmd.Parameters.AddWithValue("@SearchString", "%" + searchString.Trim() + "%");
                }

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = Convert.ToInt32(dr["ProductId"]),
                        ProductName = dr["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(dr["Quantity"]),
                        Price = Convert.ToDecimal(dr["Price"])
                    });
                }
            }

            ViewBag.SearchString = searchString;
            return View(products);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product p)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = @"INSERT INTO Products (ProductName, Quantity, Price)
                                     VALUES (@ProductName, @Quantity, @Price)";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
                    cmd.Parameters.AddWithValue("@Quantity", p.Quantity);
                    cmd.Parameters.AddWithValue("@Price", p.Price);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                TempData["Message"] = "Product added successfully.";
                return RedirectToAction("Index");
            }

            return View(p);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            Product p = new Product();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ProductId, ProductName, Quantity, Price FROM Products WHERE ProductId = @ProductId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductId", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    p.ProductId = Convert.ToInt32(dr["ProductId"]);
                    p.ProductName = dr["ProductName"].ToString();
                    p.Quantity = Convert.ToInt32(dr["Quantity"]);
                    p.Price = Convert.ToDecimal(dr["Price"]);
                }
                else
                {
                    return HttpNotFound();
                }
            }

            return View(p);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product p)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = @"UPDATE Products SET
                                     ProductName = @ProductName,
                                     Quantity = @Quantity,
                                     Price = @Price
                                     WHERE ProductId = @ProductId";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ProductId", p.ProductId);
                    cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
                    cmd.Parameters.AddWithValue("@Quantity", p.Quantity);
                    cmd.Parameters.AddWithValue("@Price", p.Price);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows == 0)
                    {
                        return HttpNotFound();
                    }
                }

                TempData["Message"] = "Product updated successfully.";
                return RedirectToAction("Index");
            }

            return View(p);
        }

        // POST: Product/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "DELETE FROM Products WHERE ProductId = @ProductId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductId", id);

                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    return HttpNotFound();
                }
            }

            TempData["Message"] = "Product deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
