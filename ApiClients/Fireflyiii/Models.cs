using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClients.Fireflyiii
{
    #region   Common
    public class FireflyApiReturn<T>
    {
        public T data { get; set; }

        public object meta { get; set; }

        public object links { get; set; }
    }

    public class FireflyApiReturnContent<T>
    {
        public string type { get; set; }

        public string id { get; set; }

        public T attributes { get; set; }
    }

    public class FireflyApiAttribute
    {
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
    #endregion

    #region   Categories
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class FireflyCategoryResponse : FireflyApiReturn<IList<FireflyApiReturnContent<Category>>> { }

    public class CategoryRelativeItem
    {
        public string currency_id { get; set; }
        public string currency_code { get; set; }
        public string currency_symbol { get; set; }
        public int? currency_decimal_places { get; set; }
        public string sum { get; set; }
    }

    public class Category
    {
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string name { get; set; }
        public string notes { get; set; }
        public string native_currency_id { get; set; }
        public string native_currency_code { get; set; }
        public string native_currency_symbol { get; set; }
        public int? native_currency_decimal_places { get; set; }
        public IList<CategoryRelativeItem> spent { get; set; }
        public IList<CategoryRelativeItem> earned { get; set; }
    }
    #endregion

    #region   Transaction
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class FireflyTransactionResponse : FireflyApiReturn<IList<FireflyApiReturnContent<FireflyTransaction>>> { }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class FireflyTransaction : FireflyApiAttribute
    {
        public string user { get; set; }

        public string group_title { get; set; }

        public IList<Transaction> transactions { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Transaction 
    {
        public string user { get; set; }
        public string transaction_journal_id { get; set; }
        public string type { get; set; }
        public DateTime? date { get; set; }
        public int? order { get; set; }
        public string currency_id { get; set; }
        public string currency_code { get; set; }
        public string currency_symbol { get; set; }
        public string currency_name { get; set; }
        public int? currency_decimal_places { get; set; }
        public string foreign_currency_id { get; set; }
        public string foreign_currency_code { get; set; }
        public string foreign_currency_symbol { get; set; }
        public int? foreign_currency_decimal_places { get; set; }
        public string amount { get; set; }
        public string foreign_amount { get; set; }
        public string description { get; set; }
        public string source_id { get; set; }
        public string source_name { get; set; }
        public string source_iban { get; set; }
        public string source_type { get; set; }
        public string destination_id { get; set; }
        public string destination_name { get; set; }
        public string destination_iban { get; set; }
        public string destination_type { get; set; }
        public string budget_id { get; set; }
        public string budget_name { get; set; }
        public string category_id { get; set; }
        public string category_name { get; set; }
        public string bill_id { get; set; }
        public string bill_name { get; set; }
        public bool? reconciled { get; set; }
        public string notes { get; set; }
        public object tags { get; set; }
        public string internal_reference { get; set; }
        public string external_id { get; set; }
        public string external_url { get; set; }
        public string original_source { get; set; }
        public string recurrence_id { get; set; }
        public int? recurrence_total { get; set; }
        public int? recurrence_count { get; set; }
        public string bunq_payment_id { get; set; }
        public string import_hash_v2 { get; set; }
        public string sepa_cc { get; set; }
        public string sepa_ct_op { get; set; }
        public string sepa_ct_id { get; set; }
        public string sepa_db { get; set; }
        public string sepa_country { get; set; }
        public string sepa_ep { get; set; }
        public string sepa_ci { get; set; }
        public string sepa_batch_id { get; set; }
        public DateTime? interest_date { get; set; }
        public DateTime? book_date { get; set; }
        public DateTime? process_date { get; set; }
        public DateTime? due_date { get; set; }
        public DateTime? payment_date { get; set; }
        public DateTime? invoice_date { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
        public int? zoom_level { get; set; }
        public bool? has_attachments { get; set; }
    }
    #endregion

}
