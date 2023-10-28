using AppData.Models;
using AppData.ViewModels;

namespace AppView.PhanTrang
{
    public class PhanTrangBienThe
    {
        public IEnumerable<BienTheViewModel> listbienthes { get; set; }=new  List<BienTheViewModel>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}
