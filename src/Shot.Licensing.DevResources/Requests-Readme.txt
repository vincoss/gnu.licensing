
## Chekc api
GET https://localhost/api/license HTTP/1.1
User-Agent: Fiddler
Host: localhost

## Health Check example
GET https://localhost/hc HTTP/1.1
User-Agent: Fiddler
Host: localhost

## Example Register from app
POST https://localhost/api/license HTTP/1.1
Host: localhost
Content-Type: application/json

{"LicenseId":"D65321D5-B0F9-477D-828A-086F30E2BF89", "ProductId":"C3F80BD7-9618-48F6-8250-65D113F9AED2", "Attributes":{"ClientId":"FAAAEB70-3BCF-4FDC-B67A-5C6B81C316C5"}}
