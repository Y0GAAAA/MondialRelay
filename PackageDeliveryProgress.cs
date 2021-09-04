using System;
namespace MondialRelay
{

    public enum PackageDeliveryProgress : int
    {
        NONE,
        PACKAGE_BEING_PREPARED,
        PACKAGE_GIVEN_TO_MONDIAL_RELAY,
        PACKAGE_BEING_PROCESSED_AT_LOGISTIC_SITE,
        PACKAGE_AVAILABLE_AT_POINT_OF_DELIVERY,
        PACKAGE_DELIVERED,
    }

    public class PackageDeliveryProgressEnumHelper
    {

        public static readonly PackageDeliveryProgress[] Values = new PackageDeliveryProgress[]{
            PackageDeliveryProgress.NONE,
            PackageDeliveryProgress.PACKAGE_BEING_PREPARED,
            PackageDeliveryProgress.PACKAGE_GIVEN_TO_MONDIAL_RELAY,
            PackageDeliveryProgress.PACKAGE_BEING_PROCESSED_AT_LOGISTIC_SITE,
            PackageDeliveryProgress.PACKAGE_AVAILABLE_AT_POINT_OF_DELIVERY,
            PackageDeliveryProgress.PACKAGE_DELIVERED,
        };

    }

}
