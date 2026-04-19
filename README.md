# NexoraERP

🧩 1. Tenant Management (SaaS Core)
📌 Epic: Tenant Provisioning & Isolation

US-TEN-01
As a system admin, I want to create a new tenant so that a new company can use the system.

US-TEN-02
As a system, I want to create a separate database per tenant so that data is isolated.

US-TEN-03
As a system, I want to run migrations automatically for new tenant databases so that schema is consistent.

US-TEN-04
As a system, I want to resolve tenant from request (subdomain / header / token) so that correct database is used.

US-TEN-05
As a system, I want to dynamically switch DbContext connection string per request so that each tenant accesses its own database.

US-TEN-06
As an admin, I want to deactivate a tenant so that access can be controlled.

🔐 2. Authentication & Authorization
📌 Epic: Identity & Access

US-AUTH-01
As a user, I want to log in so that I can access my tenant system.

US-AUTH-02
As a system, I want users to belong to a specific tenant so that cross-tenant access is prevented.

US-AUTH-03
As an admin, I want role-based access (Admin, Accountant, Viewer) so that permissions are controlled.

US-AUTH-04
As a system, I want to validate tenant context on every API request so that security is enforced.

🌳 3. Chart of Accounts (Dynamic)
📌 Epic: COA Management

US-COA-01
As an accountant, I want to create root accounts (Assets, Liabilities, Income, Expenses).

US-COA-02
As an accountant, I want to create unlimited child accounts dynamically so that hierarchy is flexible.

US-COA-03
As an accountant, I want to mark accounts as postable (GL) so that they can be used in journal entries.

US-COA-04
As a system, I must prevent adding children under GL accounts.

US-COA-05
As a user, I want to view COA as a tree structure so that I understand hierarchy.

US-COA-06
As a system, I want each tenant to have its own COA so that data is isolated.

🧾 4. Journal Entry
📌 Epic: Journal Processing

US-JRN-01
As an accountant, I want to create a journal entry so that I can record transactions.

US-JRN-02
As an accountant, I want to add multiple line items with debit/credit columns so that entries are structured.

US-JRN-03
As a system, I must ensure one line has either debit OR credit.

US-JRN-04
As a system, I must ensure total debit equals total credit before posting.

US-JRN-05
As an accountant, I want to save journal as draft so that I can complete later.

US-JRN-06
As an accountant, I want to post journal so that it becomes immutable.

US-JRN-07
As a system, I must prevent editing posted journal so that audit integrity is maintained.

US-JRN-08
As a system, I must ensure only GL accounts are used in journal lines.

💱 5. Multi-Currency
📌 Epic: Currency Management

US-MC-01
As an accountant, I want to define base currency per tenant so that reporting is consistent.

US-MC-02
As an accountant, I want to select currency per journal line.

US-MC-03
As a system, I want to store transaction currency and base currency values.

US-MC-04
As a system, I want to calculate base debit/credit using exchange rate.

US-MC-05
As a system, I must validate balance in base currency.

US-MC-06
As an admin, I want to manage exchange rates so that conversions are accurate.

🗄️ 6. EF Core & Infrastructure
📌 Epic: Data Access Layer

US-DB-01
As a developer, I want DbContext to be tenant-aware so that correct database is used per request.

US-DB-02
As a developer, I want to use dependency injection to resolve tenant-specific DbContext.

US-DB-03
As a system, I want to apply migrations per tenant database.

US-DB-04
As a system, I want connection strings stored securely so that tenant data is protected.

📊 7. Reporting
📌 Epic: Financial Reports

US-REP-01
As an accountant, I want to view trial balance per tenant.

US-REP-02
As an accountant, I want to view general ledger per account.

US-REP-03
As an accountant, I want reports in base currency so that financials are consistent.

🔍 8. Audit & Logging
📌 Epic: Audit Trail

US-AUD-01
As a system, I want to log all journal changes so that audit trail is maintained.

US-AUD-02
As a system, I want to track user actions per tenant so that accountability is ensured.

⚙️ 9. API Layer
📌 Epic: REST API

US-API-01
As a client app, I want to call APIs with tenant context so that correct data is returned.

US-API-02
As a system, I want to validate tenant ID in middleware so that unauthorized access is blocked.
