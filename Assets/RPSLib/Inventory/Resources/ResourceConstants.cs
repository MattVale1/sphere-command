/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/

namespace RPSCore {

    public enum ResourceSortingType {
        NONE,
        QUANTITY,
        RARITY,
        ALPHABETICALLY,
        ILLEGAL,
        ENUM
    }

    public enum TransactionType {
        ADD,
        SUBTRACT
    }

    public enum RarityLevel {
        COMMON,
        UNCOMMON,
        RARE,
        VERY_RARE,
        LEGENDARY,
        COSMIC,
        PROTOTYPE
    }

    public enum ItemType {
        RAW_MATERIAL,
        TRADE_GOOD,
        SHIP_COMPONENT,
        UPGRADE
    }

    public enum Currency {
        GALACTIC_CREDITS = 0,
        BIT_SHARDS = 1,
        QUANTUMITE = 2
    }

}