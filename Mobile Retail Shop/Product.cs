﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mobile_Retail_Shop
{
    public partial class Product : UserControl
    {
        private string customerID, productID, shopID;
        private int totalReviewer;
        private double totalReview;
        private Dictionary<string, CartItem> cart;
        public Product()
        {
            InitializeComponent();
        }

        public Product(string customerID, string productID, Dictionary<string, CartItem> cart) : this()
        {
            this.customerID = customerID;
            add_cart_btn.Tag = add_btn.Tag = remove_btn.Tag = this.productID = productID;
            this.cart = cart;
            DataLoad(); 
        }

        private void DataLoad()
        {
            string error, query = $"SELECT * FROM [Product Information] WHERE ID = '{this.productID}'";
            DataBase dataBase = new DataBase();
            DataTable dataTable = dataBase.DataAccess(query, out error);

            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show($"Class: Product Function: DataLoad \nError: {error}");
                return;
            }


           // product_picture.Image = Utility.ByteArrayToImage((byte[])(dataTable.Rows[0]["Picture"]));
            compnay_name.Text = dataTable.Rows[0]["Company Name"].ToString();
            model.Text = dataTable.Rows[0]["Model"].ToString();
            sim.Text = $"SIM: {dataTable.Rows[0]["SIM"]}";
            ram.Text = $"RAM: {dataTable.Rows[0]["RAM"]}";
            rom.Text = $"ROM: {dataTable.Rows[0]["ROM"]}";
            color.Text = $"COLOR: {dataTable.Rows[0]["Color"]}";
            price.Text = $"Price: {dataTable.Rows[0]["Price"]}";
            discount.Text = $"Discount: {dataTable.Rows[0]["Discount"]}";
            this.shopID = dataTable.Rows[0]["Shop ID"].ToString();

            // Convert Total Review to decimal and Total Reviewer to int
            totalReview = Convert.ToDouble(dataTable.Rows[0]["Total Review"]);
            totalReviewer = Convert.ToInt32(dataTable.Rows[0]["Total Reviewer"]);

            // Calculate the rating
            double ratingValue;

            if (totalReviewer > 0) // Ensure there are reviewers to avoid division by zero
            {
                ratingValue = (totalReview / totalReviewer) * 5; // Calculate the rating
                rating.Text = $"Rating: {ratingValue:F1} Total Review: {totalReviewer}"; // Format rating to 1 decimal place
            }
            else
                rating.Text = "Rating: N/A Total Review: 0"; // Handle case where there are no reviewers


        }

        private void add_btn_Click(object sender, EventArgs e)
        {
            CartItem cartItem = new CartItem(productId: this.productID, productName: compnay_name.Text + " " + model.Text, shopId: this.shopID, quantity: 1, price: Convert.ToDouble(price.Text.Substring(7)));
            cartItem.AddToCart(cart, productID: this.productID, productName: compnay_name.Text + " " + model.Text, shopID: this.shopID, quantity: 1, price: Convert.ToDouble(price.Text.Substring(7)));

        }

        private void remove_btn_Click(object sender, EventArgs e)
        {
            CartItem cartItem = new CartItem(productId: this.productID, productName: compnay_name.Text + " " + model.Text, shopId: this.shopID, quantity: -1, price: (-(Convert.ToDouble(price.Text.Substring(7)))));
            cartItem.AddToCart(cart, productID: this.productID, productName: compnay_name.Text + " " + model.Text, shopID: this.shopID, quantity: -1, price: (-(Convert.ToDouble(price.Text.Substring(7)))));
        }

        private void add_cart_btn_Click(object sender, EventArgs e)
        {
            Customer.Instance.panelContainer.Controls.Clear();
            CustomerDashBoardData customerDashBoardData = new CustomerDashBoardData(customerID: this.customerID, cart: this.cart);
            customerDashBoardData.Dock = DockStyle.Fill;
            Customer.Instance.panelContainer.Controls.Add(customerDashBoardData);
        }

        private void back_btn_Click(object sender, EventArgs e)
        {
            Customer.Instance.panelContainer.Controls.Clear();
            CustomerDashBoardData customerDashBoardData = new CustomerDashBoardData(customerID: this.customerID, cart: this.cart);
            customerDashBoardData.Dock = DockStyle.Fill;
            Customer.Instance.panelContainer.Controls.Add(customerDashBoardData);
        }
    }
}
