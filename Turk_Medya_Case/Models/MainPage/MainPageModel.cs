using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Turk_Medya_Case.Models.MainPage
{
    public class MainPageModel
    {
        public int errorCode { get; set; }
        public object errorMessage { get; set; }
        public List<Data>? data { get; set; }
    }

    public class Data
    {
        public string sectionType { get; set; }
        public string repeatType { get; set; }
        public int itemCountInRow { get; set; }
        public bool lazyLoadingEnabled { get; set; }
        public bool titleVisible { get; set; }
        public string title { get; set; }
        public object titleColor { get; set; }
        public string titleBgColor { get; set; }
        public string sectionBgColor { get; set; }
        public List<ItemList> itemList { get; set; }
        public int totalRecords { get; set; }
    }

    public class ItemList
    {
        public bool hasPhotoGallery { get; set; }
        public bool hasVideo { get; set; }
        public bool titleVisible { get; set; }
        public string fLike { get; set; }
        public string publishDate { get; set; }
        public string shortText { get; set; }
        public string fullPath { get; set; }
        public Category category { get; set; }
        public string videoUrl { get; set; }
        public string externalUrl { get; set; }
        public string columnistName { get; set; }
        public string itemId { get; set; }
        public string title { get; set; }
        public string imageUrl { get; set; }
        public string itemType { get; set; }
    }

    public class Category
    {
        public string categoryId { get; set; }
        public string title { get; set; }
        public string slug { get; set; }
    }
}
