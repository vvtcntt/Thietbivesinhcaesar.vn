﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Caesar.Models;
using System.Text;
namespace Caesar.Controllers.Display.Product
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/
        CaesarContext db = new CaesarContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Command(FormCollection collection, string tag)
        {
            if (collection["btnOrder"] != null)
            {

                Session["idProduct"] = collection["idPro"];
                Session["idMenu"] = collection["idCate"];
                Session["OrdProduct"] = collection["txtOrd"];
                Session["Url"] = Request.Url.ToString();
                return RedirectToAction("OrderIndex", "Order");


            }
            return View();
        }
        public ActionResult Search()
        {

            if (Session["txtSearch"] != "" && Session["txtSearch"] != null)
            {
                string plist = "";
                string idcate = Session["idCate"].ToString();
                string Name = Session["txtSearch"].ToString();
                plist += "<div class=\"Product\">";
                plist += "<div class=\"nVar_Products\">";
                plist += "<h1>" + Name + "</h1>";
                plist += "</div>";

                plist += "<div class=\"Content_Product\">";
 
                var Product = db.tblProducts.Where(p => p.Active == true && p.Name.Contains(Name)).OrderBy(p => p.Ord).ToList();
                for (int j = 0; j < Product.Count; j++)
                {
                    plist += "<div class=\"Tear_2\">";
                    plist += "<div class=\"img_thumb\">";
                    plist += "<a href=\"/1/" + Product[j].Tag + "-" + Product[j].id + ".aspx\" title=\"" + Product[j].Name + "\"><img src=\"" + Product[j].ImageLinkThumb + "\" alt=\"" + Product[j].Name + "\" /></a>";
                    plist += "</div>";
                    plist += "<a class=\"Name\" href=\"/1/" + Product[j].Tag + "-" + Product[j].id + ".aspx\" title=\"" + Product[j].Name + "\">" + Product[j].Name + "</a>";
                    plist += "<span class=\"Price\">Giá : " + string.Format("{0:#,#}", Product[j].Price) + " vnđ</span>";
                    plist += "<p class=\"PriceSale\"><span class=\"iConKM\"></span>" + string.Format("{0:#,#}", Product[j].PriceSale) + " vnđ </p>";
                    plist += "</div> ";

                }
                plist += "</div>";
                plist += "</div>";
                Session["idCate"]="";
                Session["txtSearch"] = "";
                ViewBag.Chuoisp = plist;
            }
            
            return View();

        }
        public PartialViewResult PartialProductHomes(string idCatess)
        {
            StringBuilder chuoi = new StringBuilder();
            var GroupProduct = db.tblGroupProducts.Where(p => p.Active == true && p.Priority == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < GroupProduct.Count; i++)
            {
                chuoi.Append("<div class=\"Product\">");
                chuoi.Append("<div class=\"nVar_Products\">");
                chuoi.Append("<h2> <a href=\"/0/" + GroupProduct[i].Tag + "-" + GroupProduct[i].id + ".aspx\" title=\"" + GroupProduct[i].Name + "\">" + GroupProduct[i].Name + "</a></h2>");
                chuoi.Append("<div class=\"MenuChild\">");
                int idParent = GroupProduct[i].id;
                var GroupChild = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID == idParent).OrderBy(p => p.Ord).Take(4).ToList();
                for (int j = 0; j < GroupChild.Count; j++)
                {
                    chuoi.Append("<h3><a href=\"/0/" + GroupChild[j].Tag + "-" + GroupChild[j].id + ".aspx\" title=\"" + GroupChild[j].Name + "\" rel=\"nofollow\">" + GroupChild[j].Name + "</a> </h3> ");
                }
                chuoi.Append("</div>");
                chuoi.Append("</div>"); chuoi.Append("<div class=\"Content_Product\">");
                for (int k = 0; k < GroupChild.Count; k++)
                {
                    int idCate = int.Parse(GroupChild[k].id.ToString());
                    var listProduct = db.tblProducts.Where(p => p.Active == true && p.idCate == idCate && p.ViewHomes == true).OrderBy(p => p.Ord).ToList();
                    for (int x = 0; x < listProduct.Count; x++)
                    {
                        chuoi.Append("    <div class=\"Tear_2\">");
                        chuoi.Append("<div class=\"img_thumb\">");
                        chuoi.Append("<a href=\"/1/" + listProduct[x].Tag + "-" + listProduct[x].id + ".aspx\" title=\"" + listProduct[x].Name + "\"><img src=\"" + listProduct[x].ImageLinkThumb + "\" alt=\"" + listProduct[x].Name + "\" /></a>");
                        chuoi.Append("</div>");
                        chuoi.Append("<h3><a class=\"Name\" href=\"/1/" + listProduct[x].Tag + "-" + listProduct[x].id + ".aspx\" title=\"" + listProduct[x].Name + "\">" + listProduct[x].Name + "</a></h3>");
                        chuoi.Append("<span class=\"Price\">Giá : " + string.Format("{0:#,#}", listProduct[x].Price) + " vnđ</span>");
                        chuoi.Append("<p class=\"PriceSale\"><span class=\"iConKM\"></span>" + string.Format("{0:#,#}", listProduct[x].PriceSale) + " vnđ </p>");
                        chuoi.Append(" </div> ");
                    }
                }
                chuoi.Append("</div>");
                chuoi.Append(" </div>");
            }
            tblConfig tblconfit = db.tblConfigs.First();
            ViewBag.ListProduct = chuoi;
            return PartialView(tblconfit);
        }
        List<string> Mangphantu = new List<string>();
        public List<string> Arrayid(int idParent)
        {

            var ListMenu = db.tblGroupProducts.Where(p => p.ParentID == idParent).ToList();

            for (int i = 0; i < ListMenu.Count; i++)
            {
                Mangphantu.Add(ListMenu[i].id.ToString());
                int id = int.Parse(ListMenu[i].id.ToString());
                Arrayid(id);

            }

            return Mangphantu;
        }
        string nUrl = "";
        public string UrlProduct(int idCate)
        {
            var ListMenu = db.tblGroupProducts.Where(p => p.id == idCate).ToList();
            for (int i = 0; i < ListMenu.Count; i++)
            {
                nUrl = " <a href=\"/0/" + ListMenu[i].Tag + "-" + ListMenu[i].id + ".aspx\" title=\"" + ListMenu[i].Name + "\"> " + " " + ListMenu[i].Name + "</a> <i></i>" + nUrl;
                string ids = ListMenu[i].ParentID.ToString();
                if (ids != null && ids != "")
                {
                    int id = int.Parse(ListMenu[i].ParentID.ToString());
                    UrlProduct(id);
                }
            }
            return nUrl;
        }
        public ActionResult ProductDetail(int id)
        {
            //int idp;
            //string Chuoi = tag;
            //string[] Mang = Chuoi.Split('-');
            //int one = int.Parse(Mang.Length.ToString());
            //string chuoi1 = Mang[one - 1].ToString();
            //string[] Mang1 = chuoi1.Split('.');
            //idp = int.Parse(Mang1[0].ToString());
            tblProduct product = db.tblProducts.Find(id);
            tblConfig tblconfig = db.tblConfigs.First();
            ViewBag.Title = "<title>" + product.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + product.Title + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + product.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + product.Keyword + "\" /> ";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Thietbivesinhcaesar.vn/1/"+product.Tag+"-"+product.id+".aspx\" />";
            string meta = "";
            meta += "<meta itemprop=\"name\" content=\"" + product.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + product.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"http://Thietbivesinhcaesar.vn" + product.ImageLinkThumb + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + product.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"http://Thietbivesinhcaesar.vn" + product.ImageLinkThumb + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Thietbivesinhcaesar.vn\" />";
            meta += "<meta property=\"og:description\" content=\"" + product.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta; 
           
            int idMenu = int.Parse(product.idCate.ToString());

            var GroupProduct = db.tblGroupProducts.First(p => p.id == idMenu);
            int idCate = GroupProduct.id;
            ViewBag.nUrl = "<a href=\"/\" title=\"Trang chu\" rel=\"nofollow\"><span class=\"iCon\"></span>Trang chủ</a> &raquo;" + UrlProduct(idCate) + " " + product.Name;
             
            //ListSanphamcungloai
            StringBuilder chuoisplq = new StringBuilder();
            chuoisplq.Append("<div class=\"nVar_Menu\">");
            chuoisplq.Append("<div class=\"Left_nVar_Menu\"></div>");
            chuoisplq.Append("<div class=\"Right_nVar_Menu\">");
            chuoisplq.Append("<h2>" + GroupProduct.Name + "</h2>");
            chuoisplq.Append("</div>");
            chuoisplq.Append("</div>");


            var ListGroups = db.tblGroupProducts.Where(p => p.ParentID == idCate && p.Active == true).OrderBy(p => p.Ord).ToList();
            chuoisplq.Append("<div id=\"Content_MenuLeft\">");
            chuoisplq.Append("<ul class=\"ul2\">");
            for (int i = 0; i < ListGroups.Count; i++)
            {
                chuoisplq.Append("<li class=\"li2\">");
                chuoisplq.Append("<h2><a href=\"/0/" + ListGroups[i].Tag + "-" + ListGroups[i].id + ".aspx\" title=\"" + ListGroups[i].Name + "\"><span></span> " + ListGroups[i].Name + "</a></h2>");
                chuoisplq.Append("</li>");
            }
            chuoisplq.Append("</ul>");

            chuoisplq.Append("</div>");


            ViewBag.chuoisplq = chuoisplq; string address =product.Address.ToString();
            string resultAddress = "";
            if (address != null && address != "")
            {
                int idaddress = int.Parse(address);
                if (db.tblAddresses.FirstOrDefault(p => p.id == idaddress) != null)
                    resultAddress = db.tblAddresses.FirstOrDefault(p => p.id == idaddress).Name;
            }
            ViewBag.address = resultAddress;
            return View(product);
        }
        public ActionResult ListProduct(string tag)
        {
            int idCate;
            string Chuoi = tag;
            string[] Mang = Chuoi.Split('-');
            int one = int.Parse(Mang.Length.ToString());
            string chuoi1 = Mang[one - 1].ToString();
            string[] Mang1 = chuoi1.Split('.');
            idCate = int.Parse(Mang1[0].ToString());
            tblGroupProduct groupPproduct = db.tblGroupProducts.Find(idCate);

            ViewBag.Title = "<title>" + groupPproduct.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + groupPproduct.Title + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + groupPproduct.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + groupPproduct.Keyword + "\" /> ";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Thietbivesinhcaesar.vn/0/" + groupPproduct.Tag + "-" + groupPproduct.id + ".aspx\" />";
            string meta = "";
            meta += "<meta itemprop=\"name\" content=\"" + groupPproduct.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + groupPproduct.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"http://Thietbivesinhcaesar.vn" + groupPproduct.Images + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + groupPproduct.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"http://Thietbivesinhcaesar.vn" + groupPproduct.Images + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Thietbivesinhcaesar.vn\" />";
            meta += "<meta property=\"og:description\" content=\"" + groupPproduct.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta; 
            var ListGroup = db.tblGroupProducts.Where(p =>p.Active == true && p.ParentID==idCate).OrderBy(p => p.Ord).ToList();
          StringBuilder plist=new StringBuilder();
            if(ListGroup.Count>0)
            {
                //ListGroup = db.tblGroupProducts.Where(p => p.Level.Substring(0, leghts) == levels && p.Active == true).OrderBy(p => p.Level).ToList();
                plist.Append("<span class=\"spanName\">" + groupPproduct.Name + "</span>   ");
                plist.Append("<div id=\"Content_Des\">" + groupPproduct.Content + "</div>");
                for (int i = 0; i < ListGroup.Count;i++ )
                {
                    plist.Append("<div class=\"Product\">");
                    plist.Append("<div class=\"nVar_Products\">");
                    plist.Append("<h2><a href=\"/0/" + ListGroup[i].Name + "-" + ListGroup[i].id + ".aspx\" title=\"\">"+ListGroup[i].Name+"</a></h2>");
                    plist.Append("</div>");
           
                    plist.Append("<div class=\"Content_Product\">");

                    int idCate1 = ListGroup[i].id;
                    List<string> Mangpd = new List<string>();
                    Mangpd=Arrayid(idCate1);
                    Mangpd.Add(idCate1.ToString());

                    var Product = db.tblProducts.Where(p => p.Active == true &&Mangpd.Contains(p.idCate.ToString())).OrderBy(p => p.Ord).Take(15).ToList();
                    for (int j = 0; j < Product.Count;j++ )
                    { 
                        plist.Append("<div class=\"Tear_2\">");
                        plist.Append("<div class=\"img_thumb\">");
                        plist.Append(" <a href=\"/1/" + Product[j].Tag + "-" + Product[j].id + ".aspx\" title=\"" + Product[j].Name + "\"><img src=\"" + Product[j].ImageLinkThumb + "\" alt=\"" + Product[j].Name + "\" /></a> ");
                        plist.Append("</div>");
                        plist.Append("<a class=\"Name\" href=\"/1/" + Product[j].Tag + "-" + Product[j].id + ".aspx\" title=\"" + Product[j].Name + "\">" + Product[j].Name + "</a>");
                        plist.Append("<span class=\"Price\">Giá : " + string.Format("{0:#,#}", Product[j].Price) + " vnđ</span>");
                        plist.Append("<p class=\"PriceSale\"><span class=\"iConKM\"></span>" + string.Format("{0:#,#}", Product[j].PriceSale) + " vnđ </p>");
                        plist.Append("</div> ");

                    }
                    plist.Append("</div>");
                    if(db.tblProducts.Where(p => p.Active == true &&Mangpd.Contains(p.idCate.ToString())).OrderBy(p => p.Ord).ToList().Count>15)
                    {
                        plist.Append("<div class=\"xemthem\"><a href=\"/0/" + ListGroup[i].Tag + "-" + ListGroup[i].id + ".aspx\" rel=\"nofollow\" title=\"" + ListGroup[i].Name + "\">Xem thêm sản phẩm >></a></div>");
                    }
                   
                    plist.Append("</div>");
                    Mangphantu.Clear();
                }
            }
            else
            {
                plist.Append("<span class=\"spanName\">" + groupPproduct.Name + "</span>   ");
                plist.Append("<div id=\"Content_Des\">" + groupPproduct.Content + "</div>");
            
                    plist.Append("<div class=\"Product\">");
                   
                    plist.Append("<div class=\"Content_Product\">");
                
                      
                        var Product = db.tblProducts.Where(p => p.Active == true && p.idCate == idCate).OrderBy(p => p.Ord).ToList();
                        for (int j = 0; j < Product.Count; j++)
                        {
                            plist.Append("<div class=\"Tear_2\">");
                            plist.Append("<div class=\"img_thumb\">");
                            plist.Append("<a href=\"/1/" + Product[j].Tag + "-" + Product[j].id + ".aspx\" title=\"" + Product[j].Name + "\"><img src=\"" + Product[j].ImageLinkThumb + "\" alt=\"" + Product[j].Name + "\" /></a>");
                            plist.Append("</div>");
                            plist.Append("<a class=\"Name\" href=\"/1/" + Product[j].Tag + "-" + Product[j].id + ".aspx\" title=\"" + Product[j].Name + "\">" + Product[j].Name + "</a>");
                            plist.Append("<span class=\"Price\">Giá : " + string.Format("{0:#,#}", Product[j].Price) + " vnđ</span>");
                            plist.Append("<p class=\"PriceSale\"><span class=\"iConKM\"></span>" + string.Format("{0:#,#}", Product[j].PriceSale) + " vnđ </p>");
                            plist.Append("</div> ");

                        }
                 

                    plist.Append("</div>");
                    plist.Append("</div>");

                Mangphantu.Clear();
            }
            ViewBag.chuoisp = plist;
                 
           
            ViewBag.nUrl = "<a href=\"/\" title=\"Trang chu\" rel=\"nofollow\"><span class=\"iCon\"></span>Trang chủ</a> /" + UrlProduct(idCate)+"/<h1>"+groupPproduct.Title+"</h1>";


            //Hiển thị Menu

             StringBuilder chuoisplq = new StringBuilder();
            chuoisplq.Append("<div class=\"nVar_Menu\">");
            chuoisplq.Append("<div class=\"Left_nVar_Menu\"></div>");
            chuoisplq.Append("<div class=\"Right_nVar_Menu\">");
            chuoisplq.Append("<h2>" + groupPproduct.Name + "</h2>");
            chuoisplq.Append("</div>");
            chuoisplq.Append("</div>");
            var ListGroups = db.tblGroupProducts.Where(p => p.ParentID == idCate && p.Active == true).OrderBy(p => p.Ord).ToList();
            chuoisplq.Append("<div id=\"Content_MenuLeft\">");
            chuoisplq.Append("<ul class=\"ul2\">");
            for (int i = 0; i < ListGroups.Count; i++)
            {
                chuoisplq.Append("<li class=\"li2\">");
                chuoisplq.Append("<h2><a href=\"/0/" + ListGroups[i].Tag + "-" + ListGroups[i].id + ".aspx\" title=\"" + ListGroups[i].Name + "\"><span></span> " + ListGroups[i].Name + "</a></h2>");
                int idcate1 = ListGroups[i].id;
                var listProduct1 = db.tblGroupProducts.Where(p => p.ParentID == idcate1 && p.Active == true).OrderBy(p => p.Ord).ToList();
                if (listProduct1.Count > 0)
                {

                    chuoisplq.Append("<ul class=\"ul3\">");
                    for (int j = 0; j < listProduct1.Count; j++)
                    {
                        chuoisplq.Append("<li class=\"li3\">");
                        chuoisplq.Append("<h3><a href=\"/0/" + listProduct1[j].Tag + "-" + listProduct1[j].id + ".aspx\" title=\"" + listProduct1[j].Name + "\">› " + listProduct1[j].Name + "</a></h3>");
                        chuoisplq.Append("</li>");

                    }
                    chuoisplq.Append("</ul>");
                }
                chuoisplq.Append("</li>");
            }
            chuoisplq.Append("</ul>");

            chuoisplq.Append("</div>");


            ViewBag.ChuoiMenu = chuoisplq;
            return View();
          

        }
        public ActionResult detail(string tag)
        {
            var config = db.tblConfigs.FirstOrDefault();

            ViewBag.Title = "<title> " + config.TitleSale + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + config.TitleSale + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + config.TitleSale + "\" /> ";

            var listProductSalePriority = db.tblProducts.Where(p => p.Active == true && p.Priority == true && p.ProductSale == true).OrderBy(p => p.Ord).ToList();
            StringBuilder resultPriority = new StringBuilder();
            foreach (var item in listProductSalePriority)
            {
                resultPriority.Append("<div class=\"item\">");
                resultPriority.Append("<div class=\"contentItem\">");
                resultPriority.Append("<div class=\"img\">");
                resultPriority.Append("<a href=\"/1/" + item.Tag + "-"+item.id+".aspx\" title=\"" + item.Name + "\"><img src=\"" + item.ImageLinkThumb + "\" title=\"" + item.Name + "\" /></a>");
                resultPriority.Append("</div>");
                resultPriority.Append("<a href=\"/1/" + item.Tag + "-" + item.id + ".aspx\" title=\"" + item.Name + "\" class=\"name\">" + item.Name + "</a>");
                resultPriority.Append("<div class=\"buy\">");
                resultPriority.Append("<span class=\"note\">Giá chỉ từ</span>");
                resultPriority.Append("<span class=\"price\">" + string.Format("{0:#,#}", item.PriceSale) + "<samp>đ</samp></span>");
                resultPriority.Append("<a href=\"/1/" + item.Tag + "-" + item.id + ".aspx\" title=\"" + item.Name + "\">Xem ngay  &raquo;</a>");
                resultPriority.Append("</div>");
                resultPriority.Append("</div>");
                resultPriority.Append("</div>");
            }
            ViewBag.resultPriority = resultPriority.ToString();


            var listProductSyn = db.tblProductSyns.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            if (listProductSyn.Count > 0)
            {
                ViewBag.CheckSyn = "oke";
                StringBuilder resultSyn = new StringBuilder();
                foreach (var item in listProductSyn)
                {
                    resultSyn.Append(" <div class=\"item\">");
                    resultSyn.Append("<div class=\"contentItem\">");
                    resultSyn.Append("<div class=\"img\">");
                    resultSyn.Append("<a href=\"/syn/" + item.Tag + "\" title=\"" + item.Name + "\"><img src=\"" + item.ImageLinkDetail + "\" title=\"" + item.Name + "\" /></a>");
                    resultSyn.Append(" </div>");
                    resultSyn.Append("<a href=\"/syn/" + item.Tag + "\" title=\"" + item.Name + "\" class=\"name\">" + item.Name + "</a>");
                    resultSyn.Append("<div class=\"buy\">");
                    resultSyn.Append("<span class=\"note\">Giá chỉ từ</span>");
                    resultSyn.Append("<span class=\"price\">" + string.Format("{0:#,#}", item.PriceSale) + "<samp>đ</samp></span>");
                    resultSyn.Append("<a href=\"/syn/" + item.Tag + "\" title=\"" + item.Name + "\" >Xem ngay  &raquo;</a>");
                    resultSyn.Append("</div>");
                    resultSyn.Append("</div>");
                    resultSyn.Append("</div>");
                }
                ViewBag.resultSyn = resultSyn.ToString();
            }

            var listProductSale = db.tblProducts.Where(p => p.Active == true && p.ProductSale == true).OrderBy(p => p.idCate).ToList();
            StringBuilder resultSale = new StringBuilder();
            foreach (var item in listProductSale)
            {
                resultSale.Append("<div class=\"item\">");
                resultSale.Append("<div class=\"contentItem\">");
                resultSale.Append("<div class=\"img\">");
                resultSale.Append("<a href=\"/1/" + item.Tag + "-" + item.id + ".aspx\" title=\"" + item.Name + "\"><img src=\"" + item.ImageLinkThumb + "\" title=\"" + item.Name + "\" /></a>");
                resultSale.Append("</div>");
                resultSale.Append("<a href=\"/1/" + item.Tag + "-" + item.id + ".aspx\" title=\"" + item.Name + "\" class=\"name\">" + item.Name + "</a>");
                resultSale.Append("<div class=\"buy\">");
                resultSale.Append("<span class=\"note\">Giá chỉ từ</span>");
                resultSale.Append("<span class=\"price\">" + string.Format("{0:#,#}", item.PriceSale) + "<samp>đ</samp></span>");
                resultSale.Append("<a href=\"/1/" + item.Tag + "-" + item.id + ".aspx\" title=\"" + item.Name + "\">Xem ngay  &raquo;</a>");
                resultSale.Append("</div>");
                resultSale.Append("</div>");

                resultSale.Append("</div>");
            }
            ViewBag.resultSale = resultSale.ToString();
            return View(config);
        }

    }
}
