using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CsCoreServer.Scaffold
{
    public partial class serverContext : DbContext
    {
        public serverContext()
        {
        }

        public serverContext(DbContextOptions<serverContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Apartments> Apartments { get; set; }
        public virtual DbSet<BankAccounts> BankAccounts { get; set; }
        public virtual DbSet<BankStatements> BankStatements { get; set; }
        public virtual DbSet<Bans> Bans { get; set; }
        public virtual DbSet<Crypto> Crypto { get; set; }
        public virtual DbSet<CryptoTransactions> CryptoTransactions { get; set; }
        public virtual DbSet<Dealers> Dealers { get; set; }
        public virtual DbSet<Garagekeys> Garagekeys { get; set; }
        public virtual DbSet<Gloveboxitems> Gloveboxitems { get; set; }
        public virtual DbSet<HousePlants> HousePlants { get; set; }
        public virtual DbSet<Houselocations> Houselocations { get; set; }
        public virtual DbSet<ImpoundGarage> ImpoundGarage { get; set; }
        public virtual DbSet<Lapraces> Lapraces { get; set; }
        public virtual DbSet<ManagementFunds> ManagementFunds { get; set; }
        public virtual DbSet<OccasionVehicles> OccasionVehicles { get; set; }
        public virtual DbSet<Okokbilling> Okokbilling { get; set; }
        public virtual DbSet<ParkingMeter> ParkingMeter { get; set; }
        public virtual DbSet<PhoneGallery> PhoneGallery { get; set; }
        public virtual DbSet<PhoneInvoices> PhoneInvoices { get; set; }
        public virtual DbSet<PhoneMessages> PhoneMessages { get; set; }
        public virtual DbSet<PhoneTweets> PhoneTweets { get; set; }
        public virtual DbSet<PlayerContacts> PlayerContacts { get; set; }
        public virtual DbSet<PlayerHouses> PlayerHouses { get; set; }
        public virtual DbSet<PlayerMails> PlayerMails { get; set; }
        public virtual DbSet<PlayerOutfits> PlayerOutfits { get; set; }
        public virtual DbSet<PlayerVehicles> PlayerVehicles { get; set; }
        public virtual DbSet<PlayerWarns> PlayerWarns { get; set; }
        public virtual DbSet<Players> Players { get; set; }
        public virtual DbSet<Playerskins> Playerskins { get; set; }
        public virtual DbSet<PrivateGarage> PrivateGarage { get; set; }
        public virtual DbSet<Stashitems> Stashitems { get; set; }
        public virtual DbSet<Trunkitems> Trunkitems { get; set; }
        public virtual DbSet<Vehiclekeys> Vehiclekeys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=127.0.0.1;database=server;user=root;password=Wacom1981", x => x.ServerVersion("10.7.3-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Apartments>(entity =>
            {
                entity.ToTable("apartments");

                entity.HasIndex(e => e.Citizenid)
                    .HasName("citizenid");

                entity.HasIndex(e => e.Name)
                    .HasName("name");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Citizenid)
                    .HasColumnName("citizenid")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Label)
                    .HasColumnName("label")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");
            });

            modelBuilder.Entity<BankAccounts>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .HasName("PRIMARY");

                entity.ToTable("bank_accounts");

                entity.HasIndex(e => e.Business)
                    .HasName("business");

                entity.HasIndex(e => e.Businessid)
                    .HasName("businessid");

                entity.HasIndex(e => e.Citizenid)
                    .HasName("citizenid")
                    .IsUnique();

                entity.HasIndex(e => e.Gangid)
                    .HasName("gangid");

                entity.Property(e => e.RecordId)
                    .HasColumnName("record_id")
                    .HasColumnType("bigint(255)");

                entity.Property(e => e.AccountType)
                    .IsRequired()
                    .HasColumnName("account_type")
                    .HasColumnType("enum('Current','Savings','Business','Gang')")
                    .HasDefaultValueSql("'Current'")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("bigint(255)");

                entity.Property(e => e.Business)
                    .HasColumnName("business")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Businessid)
                    .HasColumnName("businessid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Citizenid)
                    .HasColumnName("citizenid")
                    .HasColumnType("varchar(250)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Gangid)
                    .HasColumnName("gangid")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");
            });

            modelBuilder.Entity<BankStatements>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .HasName("PRIMARY");

                entity.ToTable("bank_statements");

                entity.HasIndex(e => e.Business)
                    .HasName("business");

                entity.HasIndex(e => e.Businessid)
                    .HasName("businessid");

                entity.HasIndex(e => e.Gangid)
                    .HasName("gangid");

                entity.Property(e => e.RecordId)
                    .HasColumnName("record_id")
                    .HasColumnType("bigint(255)");

                entity.Property(e => e.Account)
                    .HasColumnName("account")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Balance)
                    .HasColumnName("balance")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Business)
                    .HasColumnName("business")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Businessid)
                    .HasColumnName("businessid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Citizenid)
                    .HasColumnName("citizenid")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Deposited)
                    .HasColumnName("deposited")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Gangid)
                    .HasColumnName("gangid")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Withdraw)
                    .HasColumnName("withdraw")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Bans>(entity =>
            {
                entity.ToTable("bans");

                entity.HasIndex(e => e.Discord)
                    .HasName("discord");

                entity.HasIndex(e => e.Ip)
                    .HasName("ip");

                entity.HasIndex(e => e.License)
                    .HasName("license");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Bannedby)
                    .IsRequired()
                    .HasColumnName("bannedby")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'LeBanhammer'")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Discord)
                    .HasColumnName("discord")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Expire)
                    .HasColumnName("expire")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ip)
                    .HasColumnName("ip")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.License)
                    .HasColumnName("license")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Reason)
                    .HasColumnName("reason")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");
            });

            modelBuilder.Entity<Crypto>(entity =>
            {
                entity.HasKey(e => e.Crypto1)
                    .HasName("PRIMARY");

                entity.ToTable("crypto");

                entity.Property(e => e.Crypto1)
                    .HasColumnName("crypto")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("'qbit'")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.History)
                    .HasColumnName("history")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Worth)
                    .HasColumnName("worth")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<CryptoTransactions>(entity =>
            {
                entity.ToTable("crypto_transactions");

                entity.HasIndex(e => e.Citizenid)
                    .HasName("citizenid");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Citizenid)
                    .HasColumnName("citizenid")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("current_timestamp()");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");
            });

            modelBuilder.Entity<Dealers>(entity =>
            {
                entity.ToTable("dealers");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Coords)
                    .HasColumnName("coords")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_bin");

                entity.Property(e => e.Createdby)
                    .IsRequired()
                    .HasColumnName("createdby")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("'0'")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("'0'")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_bin");
            });

            modelBuilder.Entity<Garagekeys>(entity =>
            {
                entity.HasKey(e => e.Identifier)
                    .HasName("PRIMARY");

                entity.ToTable("garagekeys");

                entity.Property(e => e.Identifier)
                    .HasColumnName("identifier")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Keys)
                    .HasColumnName("keys")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<Gloveboxitems>(entity =>
            {
                entity.HasKey(e => e.Plate)
                    .HasName("PRIMARY");

                entity.ToTable("gloveboxitems");

                entity.HasIndex(e => e.Id)
                    .HasName("id");

                entity.Property(e => e.Plate)
                    .HasColumnName("plate")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'[]'")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Items)
                    .HasColumnName("items")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_bin");
            });

            modelBuilder.Entity<HousePlants>(entity =>
            {
                entity.ToTable("house_plants");

                entity.HasIndex(e => e.Building)
                    .HasName("building");

                entity.HasIndex(e => e.Plantid)
                    .HasName("plantid");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Building)
                    .HasColumnName("building")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Coords)
                    .HasColumnName("coords")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Food)
                    .HasColumnName("food")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'100'");

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Health)
                    .HasColumnName("health")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'100'");

                entity.Property(e => e.Plantid)
                    .HasColumnName("plantid")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Progress)
                    .HasColumnName("progress")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Sort)
                    .HasColumnName("sort")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Stage)
                    .HasColumnName("stage")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("'stage-a'")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");
            });

            modelBuilder.Entity<Houselocations>(entity =>
            {
                entity.ToTable("houselocations");

                entity.HasIndex(e => e.Name)
                    .HasName("name");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Coords)
                    .HasColumnName("coords")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Garage)
                    .HasColumnName("garage")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Label)
                    .HasColumnName("label")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Owned).HasColumnName("owned");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Tier)
                    .HasColumnName("tier")
                    .HasColumnType("tinyint(4)");
            });

            modelBuilder.Entity<ImpoundGarage>(entity =>
            {
                entity.HasKey(e => e.Garage)
                    .HasName("PRIMARY");

                entity.ToTable("impound_garage");

                entity.Property(e => e.Garage)
                    .HasColumnName("garage")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Data)
                    .HasColumnName("data")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<Lapraces>(entity =>
            {
                entity.ToTable("lapraces");

                entity.HasIndex(e => e.Raceid)
                    .HasName("raceid");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Checkpoints)
                    .HasColumnName("checkpoints")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Creator)
                    .HasColumnName("creator")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Distance)
                    .HasColumnName("distance")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Raceid)
                    .HasColumnName("raceid")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Records)
                    .HasColumnName("records")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");
            });

            modelBuilder.Entity<ManagementFunds>(entity =>
            {
                entity.ToTable("management_funds");

                entity.HasIndex(e => e.JobName)
                    .HasName("job_name")
                    .IsUnique();

                entity.HasIndex(e => e.Type)
                    .HasName("type");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("int(100)");

                entity.Property(e => e.JobName)
                    .IsRequired()
                    .HasColumnName("job_name")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasColumnType("enum('boss','gang')")
                    .HasDefaultValueSql("'boss'")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");
            });

            modelBuilder.Entity<OccasionVehicles>(entity =>
            {
                entity.ToTable("occasion_vehicles");

                entity.HasIndex(e => e.Occasionid)
                    .HasName("occasionId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Model)
                    .HasColumnName("model")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Mods)
                    .HasColumnName("mods")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Occasionid)
                    .HasColumnName("occasionid")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Plate)
                    .HasColumnName("plate")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Seller)
                    .HasColumnName("seller")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");
            });

            modelBuilder.Entity<Okokbilling>(entity =>
            {
                entity.ToTable("okokbilling");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(50)");

                entity.Property(e => e.FromId)
                    .IsRequired()
                    .HasColumnName("from_id")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.FromName)
                    .HasColumnName("from_name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Item)
                    .IsRequired()
                    .HasColumnName("item")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.LimitPayDate)
                    .HasColumnName("limit_pay_date")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("' '")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.PaidDate)
                    .HasColumnName("paid_date")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.SentDate)
                    .IsRequired()
                    .HasColumnName("sent_date")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Society)
                    .IsRequired()
                    .HasColumnName("society")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.SocietyName)
                    .IsRequired()
                    .HasColumnName("society_name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.TaxValue)
                    .HasColumnName("tax_value")
                    .HasColumnType("int(50)");

                entity.Property(e => e.ToId)
                    .IsRequired()
                    .HasColumnName("to_id")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.ToName)
                    .IsRequired()
                    .HasColumnName("to_name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("int(50)");
            });

            modelBuilder.Entity<ParkingMeter>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("parking_meter");

                entity.Property(e => e.Coord)
                    .HasColumnName("coord")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Identifier)
                    .IsRequired()
                    .HasColumnName("identifier")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.ParkCoord)
                    .HasColumnName("park_coord")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Plate)
                    .HasColumnName("plate")
                    .HasColumnType("varchar(32)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Vehicle)
                    .HasColumnName("vehicle")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<PhoneGallery>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("phone_gallery");

                entity.Property(e => e.Citizenid)
                    .IsRequired()
                    .HasColumnName("citizenid")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("current_timestamp()");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnName("image")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<PhoneInvoices>(entity =>
            {
                entity.ToTable("phone_invoices");

                entity.HasIndex(e => e.Citizenid)
                    .HasName("citizenid");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Citizenid)
                    .HasColumnName("citizenid")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Sender)
                    .HasColumnName("sender")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Sendercitizenid)
                    .HasColumnName("sendercitizenid")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Society)
                    .HasColumnName("society")
                    .HasColumnType("tinytext")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");
            });

            modelBuilder.Entity<PhoneMessages>(entity =>
            {
                entity.ToTable("phone_messages");

                entity.HasIndex(e => e.Citizenid)
                    .HasName("citizenid");

                entity.HasIndex(e => e.Number)
                    .HasName("number");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Citizenid)
                    .HasColumnName("citizenid")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Messages)
                    .HasColumnName("messages")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Number)
                    .HasColumnName("number")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");
            });

            modelBuilder.Entity<PhoneTweets>(entity =>
            {
                entity.ToTable("phone_tweets");

                entity.HasIndex(e => e.Citizenid)
                    .HasName("citizenid");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Citizenid)
                    .HasColumnName("citizenid")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("current_timestamp()");

                entity.Property(e => e.FirstName)
                    .HasColumnName("firstName")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.LastName)
                    .HasColumnName("lastName")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Picture)
                    .HasColumnName("picture")
                    .HasColumnType("varchar(512)")
                    .HasDefaultValueSql("'./img/default.png'")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.TweetId)
                    .IsRequired()
                    .HasColumnName("tweetId")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");
            });

            modelBuilder.Entity<PlayerContacts>(entity =>
            {
                entity.ToTable("player_contacts");

                entity.HasIndex(e => e.Citizenid)
                    .HasName("citizenid");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Citizenid)
                    .HasColumnName("citizenid")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Iban)
                    .IsRequired()
                    .HasColumnName("iban")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("'0'")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Number)
                    .HasColumnName("number")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");
            });

            modelBuilder.Entity<PlayerHouses>(entity =>
            {
                entity.ToTable("player_houses");

                entity.HasIndex(e => e.Citizenid)
                    .HasName("citizenid");

                entity.HasIndex(e => e.House)
                    .HasName("house");

                entity.HasIndex(e => e.Identifier)
                    .HasName("identifier");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(255)");

                entity.Property(e => e.Citizenid)
                    .HasColumnName("citizenid")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Decorations)
                    .HasColumnName("decorations")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.House)
                    .IsRequired()
                    .HasColumnName("house")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Identifier)
                    .HasColumnName("identifier")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Keyholders)
                    .HasColumnName("keyholders")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Logout)
                    .HasColumnName("logout")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Outfit)
                    .HasColumnName("outfit")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Stash)
                    .HasColumnName("stash")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");
            });

            modelBuilder.Entity<PlayerMails>(entity =>
            {
                entity.ToTable("player_mails");

                entity.HasIndex(e => e.Citizenid)
                    .HasName("citizenid");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Button)
                    .HasColumnName("button")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Citizenid)
                    .HasColumnName("citizenid")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("current_timestamp()");

                entity.Property(e => e.Mailid)
                    .HasColumnName("mailid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Read)
                    .HasColumnName("read")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Sender)
                    .HasColumnName("sender")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Subject)
                    .HasColumnName("subject")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");
            });

            modelBuilder.Entity<PlayerOutfits>(entity =>
            {
                entity.ToTable("player_outfits");

                entity.HasIndex(e => e.Citizenid)
                    .HasName("citizenid");

                entity.HasIndex(e => e.OutfitId)
                    .HasName("outfitId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Citizenid)
                    .HasColumnName("citizenid")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Model)
                    .HasColumnName("model")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.OutfitId)
                    .IsRequired()
                    .HasColumnName("outfitId")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Outfitname)
                    .IsRequired()
                    .HasColumnName("outfitname")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Skin)
                    .HasColumnName("skin")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");
            });

            modelBuilder.Entity<PlayerVehicles>(entity =>
            {
                entity.ToTable("player_vehicles");

                entity.HasIndex(e => e.Citizenid)
                    .HasName("citizenid");

                entity.HasIndex(e => e.License)
                    .HasName("license");

                entity.HasIndex(e => e.Plate)
                    .HasName("plate");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Balance)
                    .HasColumnName("balance")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Body)
                    .HasColumnName("body")
                    .HasDefaultValueSql("'1000'");

                entity.Property(e => e.Citizenid)
                    .HasColumnName("citizenid")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Depotprice)
                    .HasColumnName("depotprice")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Drivingdistance)
                    .HasColumnName("drivingdistance")
                    .HasColumnType("int(50)");

                entity.Property(e => e.Engine)
                    .HasColumnName("engine")
                    .HasDefaultValueSql("'1000'");

                entity.Property(e => e.Fakeplate)
                    .HasColumnName("fakeplate")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Financetime)
                    .HasColumnName("financetime")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Fuel)
                    .HasColumnName("fuel")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'100'");

                entity.Property(e => e.Garage)
                    .HasColumnName("garage")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Hash)
                    .HasColumnName("hash")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Impound)
                    .HasColumnName("impound")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Isparked)
                    .HasColumnName("isparked")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Job)
                    .IsRequired()
                    .HasColumnName("job")
                    .HasColumnType("varchar(32)")
                    .HasDefaultValueSql("'civ'")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.License)
                    .HasColumnName("license")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Mods)
                    .HasColumnName("mods")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_bin");

                entity.Property(e => e.ParkCoord)
                    .HasColumnName("park_coord")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Paymentamount)
                    .HasColumnName("paymentamount")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Paymentsleft)
                    .HasColumnName("paymentsleft")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Plate)
                    .IsRequired()
                    .HasColumnName("plate")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasColumnType("varchar(32)")
                    .HasDefaultValueSql("'car'")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Vehicle)
                    .HasColumnName("vehicle")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");
            });

            modelBuilder.Entity<PlayerWarns>(entity =>
            {
                entity.ToTable("player_warns");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Reason)
                    .HasColumnName("reason")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.SenderIdentifier)
                    .HasColumnName("senderIdentifier")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.TargetIdentifier)
                    .HasColumnName("targetIdentifier")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.WarnId)
                    .HasColumnName("warnId")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");
            });

            modelBuilder.Entity<Players>(entity =>
            {
                entity.HasKey(e => e.Citizenid)
                    .HasName("PRIMARY");

                entity.ToTable("players");

                entity.HasIndex(e => e.Id)
                    .HasName("id");

                entity.HasIndex(e => e.LastUpdated)
                    .HasName("last_updated");

                entity.HasIndex(e => e.License)
                    .HasName("license");

                entity.Property(e => e.Citizenid)
                    .HasColumnName("citizenid")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Charinfo)
                    .HasColumnName("charinfo")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Cid)
                    .HasColumnName("cid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Gang)
                    .HasColumnName("gang")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Inventory)
                    .HasColumnName("inventory")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Job)
                    .IsRequired()
                    .HasColumnName("job")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("current_timestamp()")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.License)
                    .IsRequired()
                    .HasColumnName("license")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Metadata)
                    .IsRequired()
                    .HasColumnName("metadata")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Money)
                    .IsRequired()
                    .HasColumnName("money")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Position)
                    .IsRequired()
                    .HasColumnName("position")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");
            });

            modelBuilder.Entity<Playerskins>(entity =>
            {
                entity.ToTable("playerskins");

                entity.HasIndex(e => e.Active)
                    .HasName("active");

                entity.HasIndex(e => e.Citizenid)
                    .HasName("citizenid");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Citizenid)
                    .IsRequired()
                    .HasColumnName("citizenid")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasColumnName("model")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Skin)
                    .IsRequired()
                    .HasColumnName("skin")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");
            });

            modelBuilder.Entity<PrivateGarage>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("private_garage");

                entity.Property(e => e.Garage)
                    .HasColumnName("garage")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Identifier)
                    .IsRequired()
                    .HasColumnName("identifier")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Inventory)
                    .HasColumnName("inventory")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Vehicles)
                    .HasColumnName("vehicles")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<Stashitems>(entity =>
            {
                entity.HasKey(e => e.Stash)
                    .HasName("PRIMARY");

                entity.ToTable("stashitems");

                entity.HasIndex(e => e.Id)
                    .HasName("id");

                entity.Property(e => e.Stash)
                    .HasColumnName("stash")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'[]'")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Items)
                    .HasColumnName("items")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_bin");
            });

            modelBuilder.Entity<Trunkitems>(entity =>
            {
                entity.HasKey(e => e.Plate)
                    .HasName("PRIMARY");

                entity.ToTable("trunkitems");

                entity.HasIndex(e => e.Id)
                    .HasName("id");

                entity.Property(e => e.Plate)
                    .HasColumnName("plate")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'[]'")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8mb3_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Items)
                    .HasColumnName("items")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_bin");
            });

            modelBuilder.Entity<Vehiclekeys>(entity =>
            {
                entity.HasKey(e => e.Plate)
                    .HasName("PRIMARY");

                entity.ToTable("vehiclekeys");

                entity.Property(e => e.Plate)
                    .HasColumnName("plate")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Keys)
                    .HasColumnName("keys")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
