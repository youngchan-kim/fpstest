[System.Serializable]
public class Inventory
{
    //Items명의 List생성 타입은 InventorySlot
    //List의 경우 게임 실행 중에 쉽게 추가와 제거가 가능하다는것 하지만 
    //public List<InventorySlot> Items = new List<InventorySlot>();
    //배열은 크기를 알아야 해당기능이 가능하다.
    //배열을 사용하려면 초기화때 배열의 크기를 설정해줘야한다.
    //처음에 배열의 크기를 8로 하지만 변경이 가능하다.
    //@슬롯
    public InventorySlot[] Items = new InventorySlot[28];
    public void Clear()
    {
        for(int i =0; i<Items.Length; i ++)
        {
            Items[i] = null;
        }
    }
}
