using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public enum PacketType
    {
        LOGIN,
        LOGIN_RESPONSE,
        CHANGE_STATUS,
        STATUS,
        AUTHENTICATION,
        AUTHENTICATION_RESPONSE,
        ERROR
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

    public enum PrivacyLevel
    {
        SELECTED_PEOPLE,
        OPEN,
        PRIVATE,
        ZERO,
        ONE,
        TWO,
        THREE
    };

    public enum ClientType
    {
        CUSTOMER,
        BAKER,
        WAREHOUSE
    }

    public enum OperationCodes
    {
        CHANGE_STATUS
    }

    public enum OrderStatus
    {
        ORDERED,
        IN_PROGRESS,
        BAKING,
        DELIVERING,
        DELIVERED
    }
}
