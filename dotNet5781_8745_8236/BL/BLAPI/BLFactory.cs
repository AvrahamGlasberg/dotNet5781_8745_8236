using BL;

namespace BLAPI
{
    public static class BLFactory
    {
        /// <summary>
        /// Static function to get implication for bl in singleton
        /// </summary>
        /// <returns>Singlton implication of BL.</returns>
        public static IBL GetBL()
        {
            try
            {
                return BLImp.Instance;
            }
            catch//creating dal/ds failed
            {
                throw new BO.MissingData("DL could not open!");
            }
        }
    }
}
