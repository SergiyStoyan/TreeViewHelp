//********************************************************************************************
//Author: Sergey Stoyan
//        sergey.stoyan@gmail.com
//        sergey_stoyan@yahoo.com
//        http://www.cliversoft.com
//        1 November 2007
//********************************************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;

namespace Cliver
{
    class Program
    {
        WebClient w = new WebClient();
        XmlDocument output = new XmlDocument();

        [STAThread]
        static void Main(string[] args)
        {
            TreeViewForm f = new TreeViewForm();
            f.ShowDialog();
        }

        void process_keys(string[] keys)
        {
            try
            {
                XmlNode a = (XmlNode)output.CreateNode(XmlNodeType.Element, "products", null);
                output.AppendChild(a);

                foreach (string key in keys)
                {
                    process_product_list("http://www.pricerunner.co.uk/search?q=" + HttpUtility.UrlEncode(key)); 
                }
                
                output.Save("output.xml");
            }
            catch (Exception e)
            {
                Console.WriteLine("### ERROR: " + e.Message);
            }
        }

        void process_product_list(string url)
        {
            try
            {
                string list_page = get_page(url);

                foreach (Match m in Regex.Matches(list_page, @"class=""productname"">.*?href=""(?'Url'.*?)""", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase))
                { 
                    process_product("http://www.pricerunner.co.uk" + HttpUtility.UrlDecode(m.Groups["Url"].Value));

                    Match npm = Regex.Match(list_page, @"href=""(?'NextPageUrl'.*?)""[^>]*?title=""Next page ", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    if (npm.Success)
                        process_product_list("http://www.pricerunner.co.uk" + HttpUtility.UrlDecode(npm.Groups["NextPageUrl"].Value));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("### ERROR: " + e.Message);
            }
        }

        void process_product(string url)
        {
            try
            {
                string product_page = get_page(url);

                string DeliveryPrice = "";
                string Name = "";
                string Price = "";
                string Url = "";
                string Description = "";
                string ImageUrl = "";
                string ImageFile = "";

                Match vm = Regex.Match(product_page, @"class=""pagetitle"">(?'Name'.*?)</h1", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                if (vm.Success)
                    Name = prepare_field(vm.Groups["Name"].Value);

                vm = Regex.Match(product_page, @"Product information-->(?'Description'.*?)(?:Read more on |</div)", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                if (vm.Success)
                    Description = prepare_field(vm.Groups["Description"].Value);

                vm = Regex.Match(product_page, @"div class=""lightbox-price"">(?'Price'.*?)</p", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                if (vm.Success)
                    Price = prepare_field(vm.Groups["Price"].Value);

                vm = Regex.Match(product_page, @"div class=""lightbox-price"">.*?<p>.*?<p>(?'DeliveryPrice'.*?)</p", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                if (vm.Success)
                    DeliveryPrice = prepare_field(vm.Groups["DeliveryPrice"].Value);

                vm = Regex.Match(product_page, @"class=""store"">.*?href=""(?'Url'.*?)""", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                if (vm.Success)
                    Url = "http://www.pricerunner.co.uk" + prepare_field(vm.Groups["Url"].Value);

                vm = Regex.Match(product_page, @"=""lightbox-product"">.*?src=""(?'ImageUrl'.*?)""", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                if (vm.Success)
                {
                    ImageUrl = prepare_field(vm.Groups["ImageUrl"].Value);
                    ImageFile = ImageUrl.Substring(ImageUrl.LastIndexOf("/") + 1);
                    w.DownloadFile(ImageUrl, ImageFile);
                }

                XmlNode n = output.CreateNode(XmlNodeType.Element, "product", null);

                XmlAttribute a = (XmlAttribute)output.CreateNode(XmlNodeType.Attribute, "Name", null);
                a.Value = Name;
                n.Attributes.Append(a);

                a = (XmlAttribute)output.CreateNode(XmlNodeType.Attribute, "PageUrl", null);
                a.Value = url;
                n.Attributes.Append(a);

                a = (XmlAttribute)output.CreateNode(XmlNodeType.Attribute, "DeliveryPrice", null);
                a.Value = DeliveryPrice;
                n.Attributes.Append(a);

                a = (XmlAttribute)output.CreateNode(XmlNodeType.Attribute, "Price", null);
                a.Value = Price;
                n.Attributes.Append(a);

                a = (XmlAttribute)output.CreateNode(XmlNodeType.Attribute, "Url", null);
                a.Value = Url;
                n.Attributes.Append(a);

                a = (XmlAttribute)output.CreateNode(XmlNodeType.Attribute, "Description", null);
                a.Value = Description;
                n.Attributes.Append(a);

                a = (XmlAttribute)output.CreateNode(XmlNodeType.Attribute, "ImageUrl", null);
                a.Value = ImageUrl;
                n.Attributes.Append(a);

                a = (XmlAttribute)output.CreateNode(XmlNodeType.Attribute, "ImageFile", null);
                a.Value = ImageFile;
                n.Attributes.Append(a);

                output.DocumentElement.AppendChild(n);
            }
            catch (Exception e)
            {
                Console.WriteLine("### ERROR: " + e.Message);
            }
        }    

        string get_page(string url)
        {
            try
            {
                Console.WriteLine("Downloading: " + url);
                return w.DownloadString(url);
            }
            catch (Exception e)
            {
                Console.WriteLine("### ERROR DOWNLOAD: " + e.Message);
            }
            return "";
        }

        string prepare_field(string str)
        {
            str = Regex.Replace(str, "<.*?>", "", RegexOptions.Compiled | RegexOptions.Singleline);
            return HttpUtility.HtmlDecode(str);
        }
    }
}
