using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Infrastructure;

namespace SportsStore.Models{
    public class SesssionCart:Cart{
        public static Cart GetCart(IServiceProvider services){
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            SesssionCart cart = session?.GetJson<SesssionCart>("Cart") ?? new SesssionCart();
            cart.Session = session;
            return cart;
        }

        [JsonIgnore]
        public ISession Session{get;set;}

        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);
            Session.SetJson("Cart", this);
        }
        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            Session.SetJson("Cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session.Remove("Cart");
        }                                                                
    }
}