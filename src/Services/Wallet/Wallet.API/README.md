Route | Verb | Usage | Other
--- | --- | ---
/api/wallet | Get | Return list of all wallets. |
/api/wallet/{id} | Get | Get wallet by id. |
/api/wallet/{id}/owner | Get | Get wallet by owner id. |
/api/wallet/{id}/owner | Post | Create a new wallet for an owner. |
/api/wallet/amount | Patch | Modify the amount of money in the wallet. | [From body] id:Guid, amount:Decimal, inc:Boolean
/api/wallet/{id} | Delete | Delete the wallet by id.
/api/wallet/{id}/owner | Delete | Delete the wallet by owner id.