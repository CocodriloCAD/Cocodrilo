using System.Collections.Generic;

namespace Cocodrilo.UserData
{
    public class ElementData
    {
        public bool mIsActive { get; set; }

        public int mBrepGoupId { get; set; }
        public List<int> mBrepGroupCouplingIds { get; set; }

        public ElementData()
        {
            mBrepGoupId = 0;
            mBrepGroupCouplingIds = new List<int> { 0 };
            mIsActive = true;
        }

        public void AddBrepGroupCouplingId(int BrepCouplingId)
        {
            if (mBrepGroupCouplingIds.Contains(BrepCouplingId))
                return;

            mBrepGroupCouplingIds.Add(BrepCouplingId);
        }


        public void AddBrepGroupCouplingIds(List<int> BrepCouplingIds)
        {
            foreach (int id in BrepCouplingIds)
            {
                if (mBrepGroupCouplingIds.Contains(id))
                    continue;
                mBrepGroupCouplingIds.Add(id);
            }
        }

        public bool IsBrepGroupCoupledWith(int BrepCouplingId)
        {
            foreach (int id in mBrepGroupCouplingIds)
            {
                if (id == BrepCouplingId)
                    return true;
            }
            return false;
        }
    }
}
