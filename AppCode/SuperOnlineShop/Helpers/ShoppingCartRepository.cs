using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml;
using SuperOnlineShop.Models;

namespace SuperOnlineShop.Helpers {
    public class ShoppingCartRepository : IShoppingCartRepository {

        public List<ShoppingCartItem> GetItems(Dictionary<int, int> itemCountPerId) {

            string connectionString = ConfigurationManager.AppSettings["umbracoDbDSN"];

            List<ShoppingCartItem> result = new List<ShoppingCartItem>();
            using (DbProviderDataContext dbContext = new DbProviderDataContext(connectionString)) {
                var xmlNodes = dbContext.cmsContentXmls.Where(item => itemCountPerId.Keys.Contains(item.nodeId)).Select(item => item.xml);
                foreach (var xmlNode in xmlNodes) {
                    result.Add(GetShoppingCartItemFromXml(xmlNode, itemCountPerId));
                }
            }
            return result;
        }

        private static ShoppingCartItem GetShoppingCartItemFromXml(string xml, Dictionary<int, int> itemCountPerId){
            ShoppingCartItem result = new ShoppingCartItem();
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            result.Id = int.Parse(doc.SelectSingleNode("Product").Attributes["id"].Value);
            result.Name = string.Format("{0} {1}",doc.SelectSingleNode("Product/producer").InnerText, doc.SelectSingleNode("Product/model").InnerText);
            result.Price = int.Parse(doc.SelectSingleNode("Product/price").InnerText);
            result.Count = itemCountPerId[result.Id];
            result.Sum = result.Price * result.Count;
            return result;
        }
    }
}