# ShoppingCart
Shopping Cart solution is REST API + client library implemented with .NET Core 2.2 and NSwag as a Swagger implementation.
By default Web API is hosted under IISExpress and can be accessed  at http://localhost:58519. This can be changed under `ShoppingCart.WebApi` > `Properties` > `Debug` > `App URL`. Swagger UI then is availible here: http://localhost:58519/swagger.

Shopping Cart Web API Client library
========
Provides HTTP client which can be used to access Shopping Cart API.

* `CartClient` - the facade to the API.
* `CartClientConfiguration`:
    - `Endpoint` - URL to the API, e.g. http://localhost:58519/api
    - `ApiKey` - secret key to authorize use of API.

Shopping Cart Web API
========
**Version:** 1.0.0

In order to use API `Authorization` header must be set to `ApiKey _your_api_key_` (e.g. `Authorization: ApiKey Chuck Norris`).


### ***POST*** /api/cart
---
**Summary:** Creates new cart and returns Id of it.

**Parameters**

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| itemData | body | Cart item data to be added to the new cart. | Yes | [ItemData](#itemdata) |

**Responses**

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 200 |  | string (guid) |
| 400 | Invalid item data |  |
| 401 | Unauthorized |  |


### ***POST***  /api/cart/{cartId}
---
**Summary:** Adds cart item to specified cart.
**Parameters**

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| cartId | path | Id of the cart. | Yes | string (guid) |
| itemData | body | Cart item data. | Yes | [ItemData](#itemdata) |

**Responses**

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 204 |  | string (guid) |
| 400 | Invalid item data |  |
| 401 | Unauthorized |  |

### ***PATCH*** /api/cart/{cartId}/items
---
**Summary:** Update quantity of specific product in the specified cart.

**Parameters**

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| cartId | path | Id of the cart. | Yes | string (guid) |
| itemData | body | Cart item data to be updated. | Yes | [ItemData](#itemdata) |

**Responses**

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 204 | Done! | string (guid) |
| 400 | Invalid item data |  |
| 401 | Unauthorized |  |
### ***GET*** /api/cart/{cartId}
---
**Summary:** Gets cart items of specified cart.

**Parameters**

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| cartId | path | Id of the cart. | Yes | string (guid) |

**Responses**

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 200 | OK  | [Cart](#cart) |
| 400 | Invalid item data |  |
| 401 | Unauthorized |  |


### ***DELETE*** /api/cart/{cartId}/items
---
**Summary:** Removes all cart items.

**Parameters**

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| cartId | path | Id of the cart. | Yes | string (guid) |

**Responses**

| Code | Description |
| ---- | ----------- |
| 204 | Done! |
| 401 | Not authorized |

### ***DELETE*** /api/cart/{cartId}/items/{productId}
---
**Summary:** Removes specified item of cart.

**Parameters**

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| cartId | path | Id of the cart. | Yes | string (guid) |
| productId | path | Id of product to be removed. | Yes | integer |

**Responses**

| Code | Description |
| ---- | ----------- |
| 204 | Done! |
| 401 | Not authorized |
| 404 | Cart not found  |

### Models
---

### Cart  

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| id | string (guid) |  | Yes |
| items | [ [CartItem](#cartitem) ] |  | No |

### CartItem  

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| productId | integer |  | Yes |
| quantity | integer |  | Yes |

### ItemData  

Cart item data.

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| productId | integer | Id of cart item product. | Yes |
| quantity | integer | Quantity of the product. | Yes |
