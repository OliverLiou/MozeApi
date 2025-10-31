# MozeApi

一個基於 ASP.NET Core 8.0 的財務管理 RESTful API，支援 Google OAuth 登入、交易記錄管理（支出/收入）、餘額調整及 URL Scheme 整合。

## 目錄

- [技術棧](#技術棧)
- [專案架構](#專案架構)
- [資料模型](#資料模型)
- [API 端點](#api-端點)
- [核心功能](#核心功能)
- [開發模式與設計原則](#開發模式與設計原則)
- [環境設定](#環境設定)
- [資料庫遷移](#資料庫遷移)
- [API 文件](#api-文件)

## 技術棧

### 核心框架
- **.NET 8.0** - 最新的 .NET 平台
- **ASP.NET Core Web API** - RESTful API 框架
- **Entity Framework Core 8.0.10** - ORM 框架
- **PostgreSQL** - 關聯式資料庫

### 身份驗證與授權
- **ASP.NET Core Identity** - 使用者管理
- **JWT Bearer Authentication** - Token 驗證
- **Google.Apis.Auth 1.70.0** - Google OAuth 2.0 登入

### 資料處理與映射
- **AutoMapper 13.0.1** - 物件對映框架
- **Newtonsoft.Json** - JSON 序列化

### API 文件
- **Swashbuckle (Swagger) 6.4.0** - API 文件自動生成
- **Swashbuckle.AspNetCore.Annotations 9.0.3** - Swagger 註解支援

## 專案架構

```
MozeApi/
├── Controllers/           # API 控制器
│   ├── AuthController.cs         # 身份驗證相關端點
│   ├── RecordController.cs       # 交易與 URL 記錄管理
│   └── WeatherForecastController.cs
├── Services/              # 業務邏輯層
│   ├── AuthService.cs            # 使用者認證服務
│   ├── GoogleAuthService.cs      # Google OAuth 服務
│   ├── JwtService.cs             # JWT Token 服務
│   ├── RecordService.cs          # 記錄管理服務（含泛型查詢）
│   └── RepositoryService.cs      # 泛型資料存取服務
├── Entities/              # 資料實體
│   ├── User.cs                   # 使用者實體（繼承 IdentityUser）
│   ├── Role.cs                   # 角色實體
│   ├── Transaction.cs            # 交易記錄實體
│   ├── AppUrl.cs                 # App URL 記錄實體
│   └── Balance.cs                # 餘額調整記錄實體
├── DTOs/                  # 資料傳輸物件
│   ├── Request/                  # 請求 DTO
│   │   ├── GoogleLoginRequest.cs
│   │   ├── CreateTransactionRequest.cs
│   │   ├── UpdateTransactionRequest.cs
│   │   ├── CreateAppUrlRequest.cs
│   │   └── UpdateAppUrlRequest.cs
│   ├── Response/                 # 回應 DTO
│   │   ├── AuthResponse.cs
│   │   ├── GoogleUserInfoResponse.cs
│   │   ├── TransactionResponse.cs
│   │   ├── AppUrlResponse.cs
│   │   ├── BalanceResponse.cs
│   │   ├── PagedResponse.cs
│   │   └── UserInfoResponse.cs
│   └── UrlScheme/                # URL Scheme DTO
│       ├── ExpenseUrlSchemeDto.cs
│       ├── IncomeUrlSchemeDto.cs
│       └── BalanceUrlSchemeDto.cs
├── Data/                  # 資料存取層
│   └── MozeContext.cs            # EF Core DbContext
├── Migrations/            # 資料庫遷移檔案
├── Helpers/               # 輔助工具
│   └── JwtHelpers.cs
├── Interface/             # 介面定義
│   └── ILogInterface.cs
├── AutoMapping.cs         # AutoMapper 設定檔
└── Program.cs             # 應用程式進入點
```

## 資料模型

### User（使用者）
繼承自 `IdentityUser`，額外欄位包括：
- `GoogleId` - Google 帳號唯一識別碼
- `Picture` - 大頭照 URL
- `CreatedAt` - 建立時間
- `LastLoginAt` - 最後登入時間
- `IsActive` - 是否啟用
- `Transactions` - 使用者的交易記錄集合（1:N）

### Transaction（交易記錄）
支援支出（Expense）與收入（Income）兩種交易類型：
- **基本資訊**: 交易類型、金額、貨幣、帳戶
- **分類資訊**: 專案、類別、子類別
- **詳細資訊**: 記錄名稱、商家、備註、標籤
- **時間資訊**: 日期（YYYY.MM.dd）、時間（HH:mm）
- **額外費用**: 手續費、手續費名稱、回饋金、回饋名稱
- **系統欄位**: 建立時間、更新時間、是否啟用（軟刪除）
- **關聯**: UserId（外鍵，關聯至 User）

### AppUrl（App URL 記錄）
用於追蹤 App URL Scheme 建立的交易：
- `Url` - URL 網址（最長 2000 字元）
- `IsFinished` - 是否已完成處理
- `TransactionId` - 關聯的交易記錄 ID（外鍵）
- `CreatedAt` / `UpdatedAt` - 時間戳記

### Balance（餘額調整記錄）
用於記錄帳戶餘額調整：
- `Account` - 帳戶名稱
- `Amount` - 調整金額
- `Date` / `Time` - 調整日期與時間
- `Note` - 備註
- `IsActive` - 是否啟用（軟刪除）

## API 端點

### 身份驗證 (`/api/Auth`)

| Method | Endpoint | 說明 |
|--------|----------|------|
| POST | `/GoogleLogin` | Google OAuth 登入 |
| POST | `/VerifyToken` | 驗證 JWT Token |

### 交易與 URL 記錄管理 (`/api/Record`)

#### Transaction（交易記錄）

| Method | Endpoint | 說明 | 功能 |
|--------|----------|------|------|
| POST | `/CreateTransaction` | 新增交易記錄 | 建立支出或收入記錄 |
| GET | `/GetTransaction/{id}` | 取得單筆交易 | 依 ID 查詢交易記錄 |
| GET | `/GetTransactions` | 取得交易列表 | 支援分頁、排序、搜尋 |
| PUT | `/UpdateTransaction/{id}` | 更新交易記錄 | 部分更新（僅更新提供的欄位）|
| DELETE | `/DeleteTransaction/{id}` | 刪除交易記錄 | 軟刪除（設定 IsActive = false）|

**GetTransactions 查詢參數**:
- `page` (預設: 1) - 頁碼
- `pageSize` (預設: 10, 最大: 100) - 每頁筆數
- `sortBy` - 排序欄位 (`amount`, `date`, `createdAt`)
- `sortOrder` (預設: `desc`) - 排序方向 (`asc`/`desc`)
- `search` - 搜尋關鍵字（使用反射自動搜尋所有字串屬性）

#### AppUrl（App URL 記錄）

| Method | Endpoint | 說明 | 功能 |
|--------|----------|------|------|
| POST | `/CreateAppUrl` | 新增 URL 記錄 | 記錄 App URL Scheme |
| GET | `/GetAppUrl/{id}` | 取得單筆 URL 記錄 | 依 ID 查詢 |
| GET | `/GetAppUrls` | 取得 URL 列表 | 支援分頁、排序、搜尋 |
| PUT | `/UpdateAppUrl/{id}` | 更新 URL 記錄 | 部分更新 |
| DELETE | `/DeleteAppUrl/{id}` | 刪除 URL 記錄 | 硬刪除（實際移除資料）|

**GetAppUrls 查詢參數**:
- `page`, `pageSize`, `sortOrder`, `search` - 同上
- `sortBy` - 排序欄位 (`isfinished`, `createdAt`)

## 核心功能

### 1. Google OAuth 2.0 身份驗證
- 使用 Google ID Token 進行使用者驗證
- 自動建立或更新使用者資料
- 發放 JWT Token 供後續 API 呼叫使用
- Token 有效期限：24 小時（可設定）

### 2. JWT Token 管理
- 產生包含使用者資訊的 JWT Token
- Token 驗證與解析
- Claims 包含：UserId, Email, UserName, Picture

### 3. 泛型查詢功能
RecordService 提供強大的泛型查詢方法 `FindAsync<TEntity, TResponse>`：
- **動態實體查詢** - 可查詢任何實體類型
- **分頁支援** - 自動計算總頁數與筆數
- **靈活排序** - 使用 Expression 指定排序欄位與方向
- **篩選條件** - 基本篩選（filter）+ 搜尋條件（searchPredicate）
- **關聯載入** - 支援多個 Include（Eager Loading）
- **自動映射** - 使用 AutoMapper 自動轉換為 Response DTO

### 4. 反射式搜尋
搜尋功能使用反射自動搜尋實體的所有字串屬性：
```csharp
searchPredicate = t => typeof(Transaction)
    .GetProperties()
    .Where(p => p.PropertyType == typeof(string) && p.GetValue(t) != null)
    .Any(p => ((string)p.GetValue(t)!).Contains(search));
```
**優點**：
- 無需手動列出所有可搜尋欄位
- 新增欄位時自動納入搜尋範圍
- 程式碼更簡潔易維護

### 5. 軟刪除 vs 硬刪除
- **Transaction**: 軟刪除（IsActive = false），資料保留可追蹤
- **AppUrl**: 硬刪除（實際移除資料），因為僅為暫存記錄

### 6. URL Scheme 支援
支援從 App URL Scheme 解析並建立交易記錄，適用於行動應用整合。

## 開發模式與設計原則

### 架構模式
- **三層式架構**: Controller → Service → Repository (DbContext)
- **依賴注入 (DI)**: 所有服務透過 DI 容器註冊與注入
- **Repository Pattern**: 泛型 RepositoryService 提供基礎資料存取
- **DTO Pattern**: 請求與回應使用獨立的 DTO，避免直接暴露實體

### 程式設計原則
- **主建構函式 (Primary Constructor)**: 使用 C# 12 語法簡化建構函式
- **可空性感知 (Nullable Reference Types)**: 啟用 nullable 防止 null 參考錯誤
- **泛型程式設計**: RecordService.FindAsync 展示泛型方法的強大與彈性
- **Expression Trees**: 使用 Lambda 表達式進行動態查詢建構
- **AutoMapper**: 自動處理物件對映，減少重複程式碼

### 服務組織原則
所有服務檔案遵循統一結構：
- 介面 (Interface) 在檔案上方
- 實作 (Implementation) 在檔案下方
- 檔案名稱不以 "I" 開頭（例如: `JwtService.cs` 而非 `IJwtService.cs`）

### API 命名規範
- 端點命名: `{Action}{Entity}` (例如: `CreateTransaction`, `GetAppUrl`)
- 動作名稱與實體名稱的首字母均大寫
- RESTful 風格路由

## 環境設定

### 必要設定檔 (`appsettings.json`)

```json
{
  "ConnectionStrings": {
    "MozeContext": "Host=localhost;Database=moze;Username=postgres;Password=your_password"
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key-at-least-32-characters-long",
    "Issuer": "moze-api",
    "Audience": "moze-api-users",
    "ExpiryInHours": 24
  },
  "GoogleAuth": {
    "ClientId": "your-google-client-id.apps.googleusercontent.com"
  },
  "Cors": {
    "AllowedOrigins": ["http://localhost:3000", "https://yourdomain.com"]
  }
}
```

### 環境變數
專案支援透過環境變數覆寫設定：
- `RAILWAY_MozeContext` - 資料庫連線字串（優先於 appsettings.json）

### 相依套件安裝

```bash
dotnet restore
```

## 資料庫遷移

### 初始化資料庫

```bash
# 建立遷移檔案
dotnet ef migrations add InitialCreate

# 套用遷移至資料庫
dotnet ef database update
```

### 新增遷移

```bash
# 當實體有變更時
dotnet ef migrations add YourMigrationName

# 套用遷移
dotnet ef database update
```

### 回復遷移

```bash
# 回復到指定的遷移
dotnet ef database update PreviousMigrationName

# 移除最後一次遷移
dotnet ef migrations remove
```

## 執行專案

### 開發模式

```bash
dotnet run
```

預設啟動於：
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

### 建置專案

```bash
dotnet build
```

### 發布專案

```bash
dotnet publish -c Release -o ./publish
```

## API 文件

專案整合 Swagger UI，提供互動式 API 文件。

### 存取 Swagger UI

啟動專案後，瀏覽：
- `http://localhost:5000/swagger`
- `https://localhost:5001/swagger`

### Swagger 功能
- 完整的 API 端點清單
- 請求/回應範例
- 互動式測試介面
- JWT Bearer Token 認證支援
- XML 註解自動生成文件

### 在 Swagger 中使用 JWT Token

1. 點擊右上角的 "Authorize" 按鈕
2. 輸入 Token（格式: `Bearer your-jwt-token`，Swagger 會自動加上 Bearer 前綴）
3. 點擊 "Authorize"
4. 現在可以測試需要身份驗證的 API

## CORS 設定

專案已設定 CORS，允許指定來源的跨域請求：
- 預設允許: `http://localhost:3000`
- 允許所有 HTTP 方法與標頭
- 允許憑證 (Credentials)

可在 `appsettings.json` 中調整 `Cors:AllowedOrigins` 設定。

## 安全性考量

1. **JWT Secret Key**: 務必使用強密鑰（至少 32 字元）並妥善保管
2. **Google Client ID**: 僅允許信任的 Client ID
3. **HTTPS**: 生產環境建議強制使用 HTTPS
4. **資料驗證**: 所有輸入均使用 Data Annotations 進行驗證
5. **SQL Injection**: 使用 EF Core 參數化查詢，有效防止 SQL 注入
6. **敏感資訊**: 避免在 Git 中提交包含敏感資訊的 appsettings.json

## 效能優化

1. **分頁查詢**: 所有列表查詢均支援分頁，限制單次最多 100 筆
2. **延遲載入**: 關聯資料使用 Include 明確載入，避免 N+1 查詢問題
3. **索引**: 建議在常用查詢欄位建立資料庫索引（UserId, TransactionType, CreatedAt 等）
4. **快取**: 可考慮引入 Redis 或記憶體快取提升讀取效能


## 授權

此專案為私有專案，未經授權不得使用或散布。

## 聯絡方式

如有問題或建議，請聯絡專案負責人。

---

**最後更新**: 2025-10-29
**版本**: 1.0.0
**框架版本**: .NET 8.0 | EF Core 8.0.10
