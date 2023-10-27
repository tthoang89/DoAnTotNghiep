using AppData.Models;

namespace AppView.PhanTrang
{
    public class PhanTrangBienThe
    {
        public IEnumerable<BienThe> listbienthes { get; set; }=new  List<BienThe>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}
