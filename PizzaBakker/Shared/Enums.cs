namespace Shared
{
    public enum PacketType
    {
        LOGIN,
        CHANGE_STATUS,
        STATUS,
        AUTHENTICATION,
        ERROR,
        ADD_INGREDIENT,
        DELETE_INGREDIENT,
        GET_LIST,
        PLACE_ORDER
    }

    public enum StatusCode
    {
        OK = 200,                   // Standard OK response.
        CREATED = 201,              // Indicates that the requested file has been created.
        ACCEPTED = 202,             // Request has been accepted.
        BAD_REQUEST = 400,          // Indicates that there was a syntax error in the request.
        UNAUTHORIZED = 401,         // Indicates that the user needs to login to perform the requested action.
        FORBIDDEN = 403,            // Indicates that the requested action is not allowed for that user.
        NOT_FOUND = 404,            // Indicates that the requested data is not found.
        INTERNAL_SERVER_ERROR = 500 // Indicates that the server ran into an error it doesn't know how to handle.

    }

    public enum ClientType
    {
        CUSTOMER,
        EMPLOYEE,
        BAKER,
        WAREHOUSE
    }

    public enum OrderStatus
    {
        ORDERED,
        PREPARING,
        DELIVERING,
        DELIVERED
    }
}
