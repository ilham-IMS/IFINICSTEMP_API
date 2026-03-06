# Prinsip-Prinsip Yang perlu diperhatikan

Dalam proses pengembangan perangkat lunak diperlukan untuk memperhatikan beberapa prinsip-prinsip agar code yang dibuat tidak hanya sekedar dapat berjalan namun mudah dipahami dan mudah untuk dikelola. prinsip-prinsip tersebut antara lain :

## SOLID

SOLID adalah akronim atau singkatan untuk lima prinsip OOP yang pertama kali diperkenalkan oleh Robert C. Martin. Prinsip-prinsip ini adalah:

**1. Single Responsibility Principle (SRP)**\
Sebuah Class atau method hanya boleh memiliki satu buah tanggung jawab

Contoh pembuatan method yang kurang baik

```csharp
  public class MasterBookRepository : IMasterBookRepository {

        Public Taks<int> BuyBook(DateTime transactionDate, decimal price, int quantity, string buyerName, string bookName)
        {
            @"Insert into book_transaction(
                         ID
                        ,transaction_date
                        ,price
                        ,quantity
                        ,buyer_name
                        ,book_id
                        ,book_name
                    )
                    values(
                          @ID
                         ,@TransactionDate
                         ,@Price
                         ,@Quantity
                         ,@BuyerName
                         ,@BookID
                         ,@BookName
                    )

                update master_book
                set    quantity = quantity - @Quantity
                where  book_ID = @BookID

                update master_cashflow
                set    cash = cash + (@Price * @Quantity)
            "
        }

  }

  public class MasterBookService : IMasterBookService {

        private readonly IMasterBookRepository _repo;

        public MasterBookService(IMasterBookRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<MasterBenchmark>> BuyBook(DateTime transactionDate, decimal price, int quantity, string byerName, string bookName)
        {

             using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
            try
            {
                var result = await _repo.BuyBook(transaction, "2024-03-04", 50000, 2, "John Doe", "Harry Potter") ;
                transaction.Commit();
                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction?.Connection?.Close();
            }
        }
  }
```

Contoh pembuatan method yang baik

```csharp
public class MasterBookRepository : IMasterBookRepository
{
    public async Task<int> InsertBookTransaction(DateTime transactionDate, decimal price, int quantity, string buyerName, string bookName)
    {
        @"Insert into book_transaction(
                         ID
                        ,transaction_date
                        ,price
                        ,quantity
                        ,buyer_name
                        ,book_id
                        ,book_name
                    )
                    values(
                          @ID
                         ,@TransactionDate
                         ,@Price
                         ,@Quantity
                         ,@BuyerName
                         ,@BookID
                         ,@BookName
                    )
        "
    }

    public async Task<int> UpdateBookQuantity(int quantity, string bookID)
    {
        @"
            update master_book
            set    quantity = @Quantity
            where  book_ID  = @BookID
        "
    }

    public async Task<int> UpdateCashflow(decimal price, int quantity)
    {
        @"
            update master_cashflow
            set    cash = cash + (@Price * @Quantity)
        "
    }
}

public class MasterBookService : IMasterBookService
{
    private readonly IMasterBookRepository _repo;

    public MasterBookService(IMasterBookRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<MasterBenchmark>> BuyBook(DateTime transactionDate, decimal price, int quantity, string buyerName, string bookName)
    {
         using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
        try
        {
            await _repo.InsertBookTransaction(transaction, transactionDate, price, quantity, buyerName, bookName);
            await _repo.UpdateBookQuantity(quantity, bookName);
            await _repo.UpdateCashflow(price, quantity);
            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            transaction?.Connection?.Close();
        }
    }
}

```

Pada cara pertama membuat method memiliki fungsi lebih dari satu yaitu insert, update, dan update dalam satu kali proses. sedangkan pada contoh method dibagi berdasarkan fungsi spesifiknya masing-masing. Cara kedua lebih baik karena sewaktu-waktu ada perubahan logika bisnis hal tersebut dapat ditagani dengan mudah.

**2. Open/Closed Principle (OCP)**\
Prilaku dari sebuah class ataupun method harus dapat dengan mudah untuk diperluas atau dikembangkan tanpa melakukan modifikasi.

```csharp
public class MasterBookRepository : IMasterBookRepository
{
    private readonly IFINContext _context;

    public MasterBookRepository(IFINContext context)
    {
        _context = context;
    }

    public async Task<int> InsertBookTransaction(DateTime transactionDate, decimal price, int quantity, string buyerName, string bookName)
    {
        var bookTransaction = new BookTransaction
        {
            ID = Guid.NewGuid(),
            TransactionDate = transactionDate,
            Price = price,
            Quantity = quantity,
            BuyerName = buyerName,
            BookID = bookName,
            BookName = bookName
        };

        _context.BookTransactions.Add(bookTransaction);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateBookQuantity(int quantity, string bookID)
    {
        var book = await _context.Books.FindAsync(bookID);
        if (book != null)
        {
            book.Quantity = quantity;
            return await _context.SaveChangesAsync();
        }
        return 0;
    }

    public async Task<int> UpdateCashflow(decimal price, int quantity)
    {
        var cashflow = await _context.Cashflows.FirstOrDefaultAsync();
        if (cashflow != null)
        {
            cashflow.Cash += price * quantity;
            return await _context.SaveChangesAsync();
        }
        return 0;
    }
}


public class MasterBookService : IMasterBookService
{
    private readonly IMasterBookRepository _repo;

    public MasterBookService(IMasterBookRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<MasterBenchmark>> BuyBook(DateTime transactionDate, decimal price, int quantity, string buyerName, string bookName)
    {
         using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
        try
        {
            await _repo.InsertBookTransaction(transaction, transactionDate, price, quantity, buyerName, bookName);
            await _repo.UpdateBookQuantity(quantity, bookName);
            await _repo.UpdateCashflow(price, quantity);
            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            transaction?.Connection?.Close();
        }
    }
}

```

Perhatikan contoh diatas dan contoh sebelumnya. Pada contoh sebelumnya query terhadap database menggunakan Micro ORM Dapper dan menggantinya menjadi Entity Framework pada contoh setelahnya. Meski mengalami perubahan layer atau class service tidak perlu mengalami perubahan. sehingga cara ini memenuhi prinsip bahwa suatu class dapat melakukan perluasan tanpa perlu melakukan modifikasi.

**3. Liskov Substitution Principle (LSP)**\
Objek dari kelas turunan harus dapat menggantikan objek dari kelas induk tanpa mempengaruhi program.

Pembuatan Interface Pada Layer Repository dan Service

```csharp

    public interface IBaseRepository<T> where T : BaseModel
    {
        IDbTransaction BeginTransaction();
        Task<List<T>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit);
        Task<T> GetRowByID(IDbTransaction transaction, string code);
        Task<int> Insert(IDbTransaction transaction, T model);
        Task<int> UpdateByID(IDbTransaction transaction, T model);
        Task<int> DeleteByID(IDbTransaction transaction, string code);
    }

    public interface IMasterBenchmarkRepository : IBaseRepository<MasterBenchmark>
    {

    }

    public interface IBaseService<T> where T : BaseModel
    {
        Task<List<T>> GetRows(string? keyword, int offset, int limit);
        Task<T> GetRowByID(string code);
        Task<int> Insert(T model);
        Task<int> UpdateByID(T model);
        Task<int> DeleteByID(string[] idList);
    }

    public interface IMasterBenchmarkService : IBaseService<MasterBenchmark>
    {

    }



```

Penerapan Liskov Subtitution Principle

```csharp

    public class MasterBenchmarkService : IMasterBenchmarkService
    {
        private readonly IMasterBenchmarkRepository _repo;

        public MasterBenchmarkService(IMasterBenchmarkRepository repo)
        {
            _repo = repo;
        }

    }

    public class MasterBenchmarkService
    {
        private readonly IBaseRepository<MasterBenchmark> _repo;

        public MasterBenchmarkService(IBaseRepository<MasterBenchmark> repo)
        {
            _repo = repo;
        }
    }
```

Pada contoh diatas `private readonly IMasterBenchmarkRepository _repo;` dapat diganti menjadi `private readonly IBaseRepository<MasterBenchmark> _repo;` tanpa mengalami kendala atau error.

**4. Interface Segregation Principle (ISP)**\
Class tidak boleh dipaksa bergantung pada suatu interface yang mereka tidak gunakan.

Contoh tidak menggunakan ISP

```csharp

    public interface IBaseService<T> where T : BaseModel
    {
        Task<List<T>> GetRows(string? keyword, int offset, int limit);
        Task<T> GetRowByID(string code);
        Task<int> Insert(T model);
        Task<int> UpdateByID(T model);
        Task<int> DeleteByID(string[] idList);
    }


```

Contoh menggunakan ISP

```csharp

    public interface IBaseService<T> where T : BaseModel
    {
        Task<T> GetRowByID(string code);
        Task<int> Insert(T model);
        Task<int> UpdateByID(T model);

    }

    public interface IGetRowsService<T> where T : BaseModel
    {
        Task<List<T>> GetRows(string? keyword, int offset, int limit);
    }

    public interface IDeleteableService<T> where T : BaseModel
    {
        Task<int> DeleteByID(string[] idList);
    }

```

Pada contoh tidak menerapkan ISP membuat suatu class yang Inherit pada IBaseService maka akan memaksanya untuk membuat atau mengimplementasikan method berikut meskipun method tersebut tidak dibutuhkan

```csharp

    public Task<List<T>> GetRows(string? keyword, int offset, int limit)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteByID(string[] listID)
    {
        throw new NotImplementedException();
    }

```

**5. Dependency Inversion Principle (DIP)**\
Modul Tingkat tinggi tidak boleh bergantung pada modul tingkat rendah. Keduanya harus bergantung pada abstraksi.

Contoh Method yang menerapkan DIP

```csharp

    public class MasterBenchmarkService : IMasterBenchmarkService
    {
        private readonly IBaseRepository<MasterBenchmark> _repo;

        public MasterBenchmarkService(IBaseRepository<MasterBenchmark> repo)
        {
            _repo = repo;
        }
    }

```

Prinsip DIP ini berkaitan dengan prinsip OCP yaitu Modul tinggat tinggi yang dalam hal ini `MasterBenchmarkService` tidak boleh ketergantungan dengan modul tinggat `MasterBenchmarkRepository` dan kedua modul harus bergantung pada bentuk Abstraksinya yaitu :

Layer Domain

```csharp

    public interface IMasterBenchmarkRepository : IBaseRepository<MasterBenchmark>, IGetRowsRepository<MasterBenchmark>, IDeleteableRepository<MasterBenchmark>
    {
        Task<int> ChangeStatus(IDbTransaction transaction, MasterBenchmark model);
    }

```

```csharp
    public interface IMasterBenchmarkService : IBaseService<MasterBenchmark>, IGetRowsService<MasterBenchmark>, IDeleteableService<MasterBenchmark>
    {
        Task<int> ChangeStatus(MasterBenchmark model);
    }

```

Dengan begitu dapat meningkatkan tingkat modularitas masing-masing layer.

## DRY

DRY adalah singkatan dari "Don't Repeat Yourself". Prinsip ini menekankan pentingnya menghindari duplikasi dalam kode dengan mengekstrak tugas-tugas umum menjadi fungsi atau metode yang dapat digunakan kembali.

Jika proses yang sama ditemukan selalu dijalankan berulang kali dan kode ditulis dengan cara yang sama, maka pembuatan fungsi atau metode yang dapat digunakan untuk merangkum kode tersebut perlu dipertimbangkan. Salah satu contoh yang sering terjadi adalah penggunaan `Auto Generated Code`

```csharp

    public string GenerateCode<T>(FormatCode formatCode, T? lastRow, string columnName)
        {
            string code = formatCode.Prefix;

            string last = "0";

            try
            {
                if (lastRow != null){

                    last = lastRow.GetType().GetProperty(columnName).GetValue(lastRow).ToString();
                }
            }
            catch (Exception)
            {
                throw new Exception($"{columnName} doesn't exist in {lastRow?.GetType().Name}");
            }

            string temp = "";
            for (int i = last.Length - 1; i >= 0; i--)
            {
                char c = last[i];

                if (!Char.IsDigit(c))
                {
                    break;
                }

                temp = last[i].ToString() + temp;
            }
            int currentNumber = int.Parse(temp) + 1;

            if (formatCode.Date != null)
            {
                code += formatCode.Delimiter + formatCode.Date.Value.ToString(formatCode.DateFormat);
            }

            code = code + formatCode.Delimiter + currentNumber.ToString($"D{formatCode.RunNumberLen}");

            return code;
        }


```

Kemudian dengan adanya sebuah method `Auto Generated Code` dengan mudah menggunakannya saat kita membutuhkan proses yang membuat code secara otomatis seperti contoh dibawah ini :

```csharp

    public async Task<int> Insert(MasterCollectibility model)
    {
        using (var transaction = _repo.BeginTransaction())
            try
            {

                var lastRowList = await _repo.GetTop(transaction, 1, 0);
                model.Code = GenerateCode(new FormatCode
                {
                    Prefix = $"COL.{model.Code}",
                    RunNumberLen = 7
                }
                , lastRowList.FirstOrDefault()
                , "Code");

                var result = await _repo.Insert(transaction, model);
                transaction.Commit();
                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction?.Connection?.Close();
            }
    }

```

## KISS

KISS adalah singkatan dari `Keep It Simple, Stupid`. Prinsip ini mendorong kesederhanaan dalam desain dan kode. Dengan menjaga kode Anda sederhana dan mudah dipahami, Anda dapat membuatnya lebih mudah untuk dikelola dan memperbaiki.

```csharp

        public async Task<List<MasterBenchmarkDetail>> GetRows(string? keyword, int offset, int limit, string idParent)
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();
            {
                try
                {
                    var result = await _repo.GetRows(transaction, keyword, offset, limit, idParent);
                    transaction.Commit();
                    return result;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    transaction?.Connection?.Close();
                }
            }
        }

```

Pada contoh method diatas dapat dilihat bagaimana code tersebut dibuat terlihat sederhana dan mudah dimengerti, mulai dari pemberian nama GetRows yang membuat programmer lain mengerti bahwa method ini memiliki tujuan untuk memngambil sekumpulan baris. kemudian method itu mengambil nilainya dari sebuah method yang ada pada \_repo.GetRows. setelah proses selesai maka transaksi tersebut akan di Commit kemudian akan menutup koneksi pada database.

## YAGNI

YAGNI adalah singkatan dari `You Aren't Gonna Need It.` Prinsip ini menyarankan untuk tidak menambahkan fungsionalitas sampai Anda benar-benar membutuhkannya.

Sebagai contoh kita ingin membuat fitur User Log yang berfungsi untuk melihat log user yang login baik secara general maupun secara spesifik, karena fitur ini bertujuan melihat/memantau log user maka kita perlu mempertimbangkan bahwa proses update dan delete tidak perlu ada didalamnya. sehingga dengan prinsip YAGNI kita tidak perlu menambahkan Method yang memiliki fungsi untuk update dan delete karena method tersebut tidak akan pernah digunakan

Contoh yang salah

```csharp

    public interface IUserLog {

        Task<List<T>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit);

        Task<T> GetRowByID(IDbTransaction transaction, string code);

        Task<int> Insert(IDbTransaction transaction, T model);

        Task<int> UpdateByID(IDbTransaction transaction, T model);

        Task<int> DeleteByID(IDbTransaction transaction, string ID);

    }

```

Contoh yang benar

```csharp

    public interface IUserLog {

        Task<List<T>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit);

        Task<T> GetRowByID(IDbTransaction transaction, string code);

        Task<int> Insert(IDbTransaction transaction, T model);

    }

```

Semua prinsip ini membantu dalam menciptakan kode yang lebih bersih, lebih mudah dipahami, dan lebih mudah dikelola.
