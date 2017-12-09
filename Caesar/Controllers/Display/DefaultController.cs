using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Caesar.Models;
using System.Text;
using Caesar;
namespace Caesar.Controllers.Display
{
    public class DefaultController : Controller
    {
        //
        // GET: /Default/
        CaesarContext db = new CaesarContext();
        public ActionResult Index()
        {
            tblConfig tblconfig = db.tblConfigs.First();
            ViewBag.Title = "<title>" + tblconfig.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + tblconfig.Title + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblconfig.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblconfig.Keywords + "\" /> ";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Thietbivesinhcaesar.vn\" />";
            string meta = "";
            meta += "<meta itemprop=\"name\" content=\"" + tblconfig.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + tblconfig.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"http://Thietbivesinhcaesar.vn" + tblconfig.Logo + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + tblconfig.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"http://Thietbivesinhcaesar.vn" + tblconfig.Logo + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Thietbivesinhcaesar.vn\" />";
            meta += "<meta property=\"og:description\" content=\"" + tblconfig.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta; 
            Session["h1"] = "<h1 class=\"h1\">" + tblconfig.Title + "</h1>";
            return View();
        }
        public PartialViewResult PartialMenuMobile()
        {
            StringBuilder chuoi = new StringBuilder();
            var listMenu = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID == null).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < listMenu.Count; i++)
            {
                chuoi.Append("<li> <a href=\"/0/" + listMenu[i].Tag + "-" + listMenu[i].id + ".aspx\">" + listMenu[i].Name + "</a>");
                int idcate1 = listMenu[i].id;
                var LitsMenu1 = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID == idcate1).OrderBy(p => p.Ord).ToList();
                if (LitsMenu1.Count > 0)
                {
                    chuoi.Append("<ul>");
                    for (int j = 0; j < LitsMenu1.Count; j++)
                    {
                        chuoi.Append("<li><a href=\"/0/" + LitsMenu1[j].Tag + "-" + LitsMenu1[j].id + ".aspx \">" + LitsMenu1[j].Name + "</a>");

                        int idcate2 = LitsMenu1[j].id;
                        var Listmenu2 = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID == idcate2).OrderBy(p => p.Ord).ToList();
                        if (Listmenu2.Count > 0)
                        {
                            chuoi.Append(" <ul>");
                            for (int k = 0; k < Listmenu2.Count; k++)
                            {
                                chuoi.Append("<li><a href=\"/0/" + Listmenu2[k].Tag + "-" + Listmenu2[k].id + ".aspx\">" + Listmenu2[k].Name + "</a></li>");
                            }
                            chuoi.Append(" </ul>");
                        }
                        chuoi.Append("</li>");
                    }
                    chuoi.Append("</ul>");
                }
                chuoi.Append("</li>");
            }
            ViewBag.chuoimenu = chuoi;
            return PartialView();
        }
        public ActionResult Search(FormCollection collection)
        {
            if (collection["btnSearch"] != null)
            {
                if (collection["txtSearch"] != "")
                {
                    string Cate = collection["drMenu"];
                    string txtSearch = collection["txtSearch"];
                    Session["idCate"] = Cate;
                    Session["txtSearch"] = txtSearch;
                    return Redirect("/SearchProduct");
                }
            }
            return View();
        }
        public PartialViewResult PartialTop()
        {
            tblConfig tblconfig = db.tblConfigs.First();
            int Date1 = int.Parse(DateTime.Now.Hour.ToString());
           
                ViewBag.Chuoihotline = " <p><span class=\"icon_Hotline\"></span> : Hotline " + tblconfig.HotlineIN + " - Tổng đài bán hàng : " + tblconfig.HotlineOUT + "</p><p><span class=\"icon_Phone\"></span> : " + tblconfig.MobileIN + "</p>";
            
            
             
            var ListSupport = db.tblSupports.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            string Yahoo = "";
            string Skype = "";
            for (int i = 0; i < ListSupport.Count;i++ )
            {
                //Yahoo
                Yahoo += "<div class=\"Tear_Yahoo\">";
                Yahoo += "<div class=\"Left_Tear_Yahoo\">";
                Yahoo += "<span class=\"Func\">" + ListSupport[i].Name + " :</span>";
                Yahoo += "</div>";
                Yahoo += "<div class=\"Right_Tear_Yahoo\">";
                Yahoo += "<a href=\"ymsgr:sendim?" + ListSupport[i].Yahoo + "\">";
                Yahoo += "<img src=\"http://opi.yahoo.com/online?u=" + ListSupport[i].Yahoo + "&m=g&t=1\" alt=\"Yahoo\" class=\"imgYahoo\" />";
                Yahoo += "</a>";
                Yahoo += "</div>";
                Yahoo += "</div> ";
                //Skype
                Skype += "<div class=\"Tear_Skype\">";
                Skype += "<div class=\"Left_Tear_Skype\">";
                Skype += "<span class=\"Func\">" + ListSupport[i].Name + " :</span>";
                Skype += "</div>";
                Skype += "<div class=\"Right_Tear_Skype\">";
                Skype += "<a href=\"Skype:" + ListSupport[i].Skyper + "?chat\">";
                Skype += "<img class=\"imgSkyper\" src=\"/Content/Display/iCon/skype-icon.png\" title=\"" + ListSupport[i].Name + "\" alt=\"" + ListSupport[i].Skyper + "\">";
                Skype += "</a>";
                Skype += "</div>";
                Skype += "</div>";

            }
            ViewBag.yahoo = Yahoo;
            ViewBag.skype = Skype;
            if (Session["h1"]!=null)
            {
                ViewBag.h1 = Session["h1"];
                Session["h1"] = null;
            }
                return PartialView(tblconfig);
        }
        public PartialViewResult PartialBanner()
        {
            tblConfig tblconfig = db.tblConfigs.First();
            return PartialView(tblconfig);
        }
        public PartialViewResult PartialMenu()
        {
            StringBuilder Menu = new StringBuilder();
            var ListMenu = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID == null).OrderBy(p => p.Ord).ToList();
            Menu.Append(" <ul class=\"ul_2\">");
            for (int i = 0; i < ListMenu.Count; i++)
            {
                Menu.Append("  <li class=\"li_2\">");
                Menu.Append(" <a href=\"/0/" + ListMenu[i].Tag + "-" + ListMenu[i].id + ".aspx\" title=\"" + ListMenu[i].Name + "\" rel=\"nofollow\"> " + ListMenu[i].Name + "</a>");
                int idCate = ListMenu[i].id;
                var listMenu1 = db.tblGroupProducts.Where(p => p.ParentID == idCate && p.Active == true).OrderBy(p => p.Ord).ToList();
                if (listMenu1.Count > 0)
                {
                    Menu.Append(" <ul class=\"ul_3\">");
                    for (int j = 0; j < listMenu1.Count; j++)
                    {
                        Menu.Append(" <li class=\"li_3\">");
                        Menu.Append(" <a href=\"/0/" + listMenu1[j].Tag + "-" + listMenu1[j].id + ".aspx\" title=\"" + listMenu1[j].Name + "\"  rel=\"nofollow\"> <span class=\"iCon\"></span>" + listMenu1[j].Name + "</a>");
                        int idCate1 = listMenu1[j].id;
                        var ListMenu2 = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID == idCate1).OrderBy(p => p.Ord).ToList();
                        if (ListMenu2.Count > 0)
                        {
                            Menu.Append(" <ul class=\"ul_4\">");
                            for (int k = 0; k < ListMenu2.Count; k++)
                            {
                                Menu.Append(" <li class=\"li_4\">");
                                Menu.Append(" <a href=\"/0/" + ListMenu2[k].Tag + "-" + ListMenu2[k].id + ".aspx\" title=\"" + ListMenu2[k].Name + "\"  rel=\"nofollow\">› " + ListMenu2[k].Name + "</a>");
                                Menu.Append(" </li>");
                            }
                            Menu.Append(" </ul>");
                        }
                        Menu.Append(" </li>");
                    }
                    Menu.Append(" </ul>");
                }
                Menu.Append(" </li>");
            }
              Menu.Append(" </ul>");
            ViewBag.Menu = Menu;
                return PartialView();
        }
        public PartialViewResult PartialHeader()
        {
            var listImage = db.tblImages.Where(p => p.Active == true && p.idCate == 1).OrderBy(p => p.Ord).ToList();
            StringBuilder chuoi = new StringBuilder();
            for (int i = 0; i < listImage.Count; i++)
            {
                if (i == 0)
                {
                    chuoi.Append("url(" + listImage[i].Images + ") 0 0 no-repeat");
                }
                else
                    chuoi.Append("," + "url(" + listImage[i].Images + ") " + 586 * i + "px 0 no-repeat");
            }
            ViewBag.chuoi = chuoi;
            var video = db.tblVideos.Where(p => p.Active == true).OrderByDescending(p => p.Ord).Take(1).ToList();
            StringBuilder chuoivideo = new StringBuilder();
            //if (video[0].AutoPlay == true)
            //{
            //    chuoivideo.Append(" <iframe width=\"100%\" height=\"242px\" src=\"http://www.youtube.com/embed/" + video[0].Code + "?;hl=en&amp;fs=1&amp;autoplay=1;loop=1;repeat=0;rel=0\" frameborder=\"0\" allowfullscreen></iframe>");
            //}
            //else
            //{
                chuoivideo.Append(" <iframe width=\"100%\" height=\"242px\" src=\"http://www.youtube.com/embed/" + video[0].Code + "?;hl=en&amp;fs=1&amp;autoplay=0;loop=1;repeat=0;rel=0\" frameborder=\"0\" allowfullscreen></iframe>");
            //}
            ViewBag.chuoivideo = chuoivideo;
            return PartialView(listImage);
      
        }
        public PartialViewResult PartialCenter_Headder()
        {
             var listimages = db.tblImages.Where(p => p.Active == true && p.idCate == 6).OrderBy(p => p.Ord).ToList();

             return PartialView(listimages);
        }
        public PartialViewResult PartialFootter()
        {
            tblConfig tblconfig = db.tblConfigs.First();

            

             string chuoipartner = "";
            var listPartner = db.tblPartners.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < listPartner.Count;i++ )
            { 
                 chuoipartner+="<h4>"+listPartner[i].Name+"</h4>";
                 chuoipartner+="<span class=\"Address_Company\">Địa chỉ : "+listPartner[i].Address+"</span>";
                 chuoipartner+="<span class=\"Mobile_Company\">Điện thoại :"+listPartner[i].Mobile+" Hotline : "+listPartner[i].Hotline+"</span>";
                 chuoipartner+="<span class=\"Email_Company\">Email : "+listPartner[i].Email+"</span>";
                
            }
            ViewBag.chuoipartner = chuoipartner;


            //sản phẩm chính\

            var listsanphamchinh = db.tblGroupProducts.Where(p => p.Active == true && p.Priority==true).OrderBy(p => p.Ord).ToList();
            string sanphamchinh = "";
            for (int i = 0; i < listsanphamchinh.Count;i++ )
            {

                sanphamchinh += "<a href=\"/0/"+listsanphamchinh[i].Tag+"-"+listsanphamchinh[i].id+".aspx\" title=\""+listsanphamchinh[i].Name+"\">"+listsanphamchinh[i].Name+"</a>";
            }
            ViewBag.sanphamchinh = sanphamchinh;


            //Chính sách dịch vụ
            var listchinhsach = db.tblNews.Where(p => p.Active == true && p.idCate==19).OrderBy(p => p.Ord).ToList();
            string chinhsach = "";
            for (int i = 0; i < listchinhsach.Count; i++)
            {

                chinhsach += "<a href=\"/2/" + listchinhsach[i].Tag + "-" + listchinhsach[i].id + ".aspx\" rel=\"nofollow\" title=\"" + listchinhsach[i].Name + "\">" + listchinhsach[i].Name + "</a>";
            }
            ViewBag.chinhsach = chinhsach;

            // Load Maps
            var map = db.tblMaps.First();
            ViewBag.maps = map.Content;

            var Imagesadw = db.tblImages.Where(p => p.Active == true && p.idCate == 7).OrderByDescending(p => p.Ord).Take(1).ToList();
            if (Imagesadw.Count > 0)
                ViewBag.Chuoiimg = "<a href=\"" + Imagesadw[0].Url + "\" title=\"" + Imagesadw[0].Name + "\"><img src=\"" + Imagesadw[0].Images + "\" alt=\"" + Imagesadw[0].Name + "\" style=\"max-width:100%;\" /> </a>";
             return PartialView(tblconfig);
        }
        public PartialViewResult Productdb()
        {
            var spdongbo = db.tblProducts.Where(p => p.Active == true && p.idCate == 126 && p.ViewHomes==true).OrderBy(p => p.Ord).ToList();
            string chuoi="";
            if(spdongbo.Count>0)
            {
            chuoi+="<div id=\"Product_Db\">   ";
             chuoi+="<div class=\"nVar_Product\">   ";
                 chuoi+="<div class=\"Left_nVar_Product\"></div>";
                 chuoi+="<div class=\"Center_nVar_Product\">   ";
                     chuoi+="<span>Sản phẩm đồng bộ</span>   ";
                 chuoi+="</div>   ";
                 chuoi+="<div class=\"Right_nVar_Product\"></div>   ";
             chuoi+="</div>   ";
             chuoi+="<div id=\"Content_Product_Db\">   ";
                for(int i=0;i<spdongbo.Count;i++)
                {
                 chuoi+="<div class=\"Tear_1\">   ";
                     chuoi+="<div class=\"spdb\"></div>   ";
                     chuoi+="<div class=\"img_thumb\">   ";
                         chuoi+="<a href=\"/1/"+spdongbo[i].Tag+"-"+spdongbo[i].id+".aspx\" title=\""+spdongbo[i].Name+"\"><img src=\""+spdongbo[i].ImageLinkThumb+"\" title=\""+spdongbo[i].Name+"\" /></a>   ";
                     chuoi+="</div>   ";
                    chuoi += "<a class=\"Name\" href=\"/1/"+spdongbo[i].Tag+"-"+spdongbo[i].id+".aspx\" title=\"" + spdongbo[i].Name + "\">" + spdongbo[i].Name + "</a>   ";
                     chuoi+="<span class=\"Headshort\"> › "+spdongbo[i].Description+"</span>   ";
                     chuoi += "<span class=\"Price\">Giá : " + string.Format("{0:#,#}", spdongbo[i].Price) + " vnđ</span>   ";
                     chuoi += "<span class=\"PriceSale\">Khuyến mại : " + string.Format("{0:#,#}", spdongbo[i].Price) + " vnđ</span>   ";
                 chuoi+="</div>   ";
                }

             chuoi+="</div>   ";
            chuoi+="</div>   ";
        }
            ViewBag.productdb = chuoi;
            return PartialView( );
        }
        //Homes

        //Count Online
       
    }
}
