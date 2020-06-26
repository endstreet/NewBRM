using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Sassa.BRM.Data.Models;

namespace Sassa.BRM.Data.Context
{
    public partial class DcContext : DbContext
    {
        public DcContext()
        {
        }

        public DcContext(DbContextOptions<DcContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DcActivity> DcActivity { get; set; }
        public virtual DbSet<DcBatch> DcBatch { get; set; }
        public virtual DbSet<DcBoxType> DcBoxType { get; set; }
        public virtual DbSet<DcBoxpicked> DcBoxpicked { get; set; }
        public virtual DbSet<DcDestruction> DcDestruction { get; set; }
        public virtual DbSet<DcDestructionList> DcDestructionList { get; set; }
        public virtual DbSet<DcDistrict> DcDistrict { get; set; }
        public virtual DbSet<DcDistrictEc> DcDistrictEc { get; set; }
        public virtual DbSet<DcDocumentType> DcDocumentType { get; set; }
        public virtual DbSet<DcExclusions> DcExclusions { get; set; }
        public virtual DbSet<DcFile> DcFile { get; set; }
        public virtual DbSet<DcFileRec> DcFileRec { get; set; }
        public virtual DbSet<DcFileRequest> DcFileRequest { get; set; }
        public virtual DbSet<DcGrantDocLink> DcGrantDocLink { get; set; }
        public virtual DbSet<DcGrantType> DcGrantType { get; set; }
        public virtual DbSet<DcLcType> DcLcType { get; set; }
        public virtual DbSet<DcLocalOffice> DcLocalOffice { get; set; }
        public virtual DbSet<DcMerge> DcMerge { get; set; }
        public virtual DbSet<DcOfficeKuafLink> DcOfficeKuafLink { get; set; }
        public virtual DbSet<DcPicklist> DcPicklist { get; set; }
        public virtual DbSet<DcRegion> DcRegion { get; set; }
        public virtual DbSet<DcReqCategory> DcReqCategory { get; set; }
        public virtual DbSet<DcReqCategoryType> DcReqCategoryType { get; set; }
        public virtual DbSet<DcReqCategoryTypeLink> DcReqCategoryTypeLink { get; set; }
        public virtual DbSet<DcStakeholder> DcStakeholder { get; set; }
        public virtual DbSet<DcTransactionType> DcTransactionType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseOracle("Data Source = (DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.117.122.120)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=ecstrn))); user id=contentserver; password=Password123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "CONTENTSERVER");

            modelBuilder.Entity<DcActivity>(entity =>
            {
                entity.ToTable("DC_ACTIVITY");

                entity.HasIndex(e => e.DcActivityId)
                    .HasName("SYS_C0018987")
                    .IsUnique();

                entity.Property(e => e.DcActivityId)
                    .HasColumnName("DC_ACTIVITY_ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Activity)
                    .IsRequired()
                    .HasColumnName("ACTIVITY")
                    .HasMaxLength(255);

                entity.Property(e => e.ActivityDate)
                    .HasColumnName("ACTIVITY_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("CURRENT_DATE ");

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasColumnName("AREA")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.OfficeId)
                    .HasColumnName("OFFICE_ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.RegionId)
                    .IsRequired()
                    .HasColumnName("REGION_ID")
                    .HasMaxLength(50);

                entity.Property(e => e.Result)
                    .IsRequired()
                    .HasColumnName("RESULT")
                    .HasMaxLength(255);

                entity.Property(e => e.UnqFileNo)
                    .HasColumnName("UNQ_FILE_NO")
                    .HasMaxLength(20);

                entity.Property(e => e.Userid)
                    .HasColumnName("USERID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("USERNAME")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<DcBatch>(entity =>
            {
                entity.HasKey(e => e.BatchNo);

                entity.ToTable("DC_BATCH");

                entity.HasIndex(e => e.BatchNo)
                    .HasName("PK_DC_BATCH")
                    .IsUnique();

                entity.HasIndex(e => e.BatchStatus)
                    .HasName("INDEX19");

                entity.HasIndex(e => e.OfficeId)
                    .HasName("INDEX18");

                entity.HasIndex(e => e.RegType)
                    .HasName("INDEX20");

                entity.HasIndex(e => e.UpdatedBy)
                    .HasName("INDEX17");

                entity.Property(e => e.BatchNo)
                    .HasColumnName("BATCH_NO")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.BatchComment)
                    .HasColumnName("BATCH_COMMENT")
                    .HasColumnType("VARCHAR2(500)");

                entity.Property(e => e.BatchCurrent)
                    .HasColumnName("BATCH_CURRENT")
                    .HasColumnType("CHAR(1)");

                entity.Property(e => e.BatchStatus)
                    .HasColumnName("BATCH_STATUS")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.CourierName)
                    .HasColumnName("COURIER_NAME")
                    .HasColumnType("VARCHAR2(255)");

                entity.Property(e => e.OfficeId)
                    .HasColumnName("OFFICE_ID")
                    .HasColumnType("VARCHAR2(10)");

                entity.Property(e => e.RegType)
                    .HasColumnName("REG_TYPE")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("UPDATED_BY")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.UpdatedByAd)
                    .HasColumnName("UPDATED_BY_AD")
                    .HasColumnType("VARCHAR2(225)");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("UPDATED_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.WaybillDate)
                    .HasColumnName("WAYBILL_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.WaybillNo)
                    .HasColumnName("WAYBILL_NO")
                    .HasColumnType("VARCHAR2(20)");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.DcBatch)
                    .HasForeignKey(d => d.OfficeId)
                    .HasConstraintName("FK_BATCH_LOCAL_OFFICE");
            });

            modelBuilder.Entity<DcBoxType>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DC_BOX_TYPE");

                entity.Property(e => e.BoxType)
                    .IsRequired()
                    .HasColumnName("BOX_TYPE")
                    .HasColumnType("VARCHAR2(100)");

                entity.Property(e => e.BoxTypeId)
                    .HasColumnName("BOX_TYPE_ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.IsTransport)
                    .IsRequired()
                    .HasColumnName("IS_TRANSPORT")
                    .HasColumnType("VARCHAR2(1)");
            });

            modelBuilder.Entity<DcBoxpicked>(entity =>
            {
                entity.HasKey(e => e.UnqNo)
                    .HasName("DC_BOXPICKED_PK");

                entity.ToTable("DC_BOXPICKED");

                entity.HasIndex(e => e.BinNumber)
                    .HasName("INDEX9");

                entity.HasIndex(e => e.BoxNumber)
                    .HasName("INDEX10");

                entity.HasIndex(e => e.UnqNo)
                    .HasName("DC_BOXPICKED_PK")
                    .IsUnique();

                entity.HasIndex(e => e.UnqPicklist)
                    .HasName("INDEX8");

                entity.Property(e => e.UnqNo)
                    .HasColumnName("UNQ_NO")
                    .HasColumnType("VARCHAR2(20)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ArchiveYear)
                    .HasColumnName("ARCHIVE_YEAR")
                    .HasColumnType("VARCHAR2(4)");

                entity.Property(e => e.BinNumber)
                    .HasColumnName("BIN_NUMBER")
                    .HasColumnType("VARCHAR2(10)");

                entity.Property(e => e.BoxCompleted)
                    .HasColumnName("BOX_COMPLETED")
                    .HasColumnType("CHAR(1)");

                entity.Property(e => e.BoxNumber)
                    .IsRequired()
                    .HasColumnName("BOX_NUMBER")
                    .HasColumnType("VARCHAR2(10)");

                entity.Property(e => e.BoxReceived)
                    .HasColumnName("BOX_RECEIVED")
                    .HasColumnType("CHAR(1)");

                entity.Property(e => e.UnqPicklist)
                    .IsRequired()
                    .HasColumnName("UNQ_PICKLIST")
                    .HasColumnType("VARCHAR2(20)");
            });

            modelBuilder.Entity<DcDestruction>(entity =>
            {
                entity.HasKey(e => e.PensionNo)
                    .HasName("DC_DESTRUCTION_PK");

                entity.ToTable("DC_DESTRUCTION");

                entity.HasIndex(e => e.PensionNo)
                    .HasName("DC_DESTRUCTION_PK")
                    .IsUnique();

                entity.Property(e => e.PensionNo)
                    .HasColumnName("PENSION_NO")
                    .HasMaxLength(20);

                entity.Property(e => e.DestructioDate)
                    .HasColumnName("DESTRUCTIO_DATE")
                    .HasMaxLength(20)
                    .HasDefaultValueSql("TO_CHAR(SYSDATE,'YYYYMMDD')");

                entity.Property(e => e.DestructionBatchId)
                    .HasColumnName("DESTRUCTION_BATCH_ID")
                    .HasColumnType("NUMBER(38)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ExclusionbatchId)
                    .HasColumnName("EXCLUSIONBATCH_ID")
                    .HasColumnType("NUMBER(38)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.GrantType)
                    .HasColumnName("GRANT_TYPE")
                    .HasColumnType("VARCHAR2(5)");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasColumnType("VARCHAR2(60)");

                entity.Property(e => e.RegionId)
                    .HasColumnName("REGION_ID")
                    .HasColumnType("NUMBER(38)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("STATUS")
                    .HasMaxLength(20)
                    .HasDefaultValueSql("'Selected'");

                entity.Property(e => e.StatusDate)
                    .IsRequired()
                    .HasColumnName("STATUS_DATE")
                    .HasMaxLength(20)
                    .HasDefaultValueSql("TO_CHAR(SYSDATE,'YYYYMMDD')");

                entity.Property(e => e.Surname)
                    .HasColumnName("SURNAME")
                    .HasColumnType("VARCHAR2(60)");
            });

            modelBuilder.Entity<DcDestructionList>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DC_DESTRUCTION_LIST");

                entity.HasIndex(e => e.GrantType)
                    .HasName("DC_DESTRUCTION_LIST_INDX05");

                entity.HasIndex(e => e.Name)
                    .HasName("DC_DESTRUCTION_LIST_INDX03");

                entity.HasIndex(e => e.PensionNo)
                    .HasName("DC_DESTRUCTION_LIST_INDX02");

                entity.HasIndex(e => e.Region)
                    .HasName("DC_DESTRUCTION_LIST_INDX01");

                entity.HasIndex(e => e.StatusDate)
                    .HasName("DC_DESTRUCTION_LIST_INDX06");

                entity.HasIndex(e => e.Surname)
                    .HasName("DC_DESTRUCTION_LIST_INDX04");

                entity.Property(e => e.GrantType)
                    .HasColumnName("GRANT_TYPE")
                    .HasColumnType("VARCHAR2(5)");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasColumnType("VARCHAR2(60)");

                entity.Property(e => e.PensionNo)
                    .HasColumnName("PENSION_NO")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.Region)
                    .HasColumnName("REGION")
                    .HasColumnType("VARCHAR2(5)");

                entity.Property(e => e.StatusDate)
                    .HasColumnName("STATUS_DATE")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.Surname)
                    .HasColumnName("SURNAME")
                    .HasColumnType("VARCHAR2(60)");
            });

            modelBuilder.Entity<DcDistrict>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DC_DISTRICT");

                entity.Property(e => e.District)
                    .IsRequired()
                    .HasColumnName("DISTRICT")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.DistrictName)
                    .HasColumnName("DISTRICT_NAME")
                    .HasColumnType("VARCHAR2(255)");

                entity.Property(e => e.RegionId)
                    .HasColumnName("REGION_ID")
                    .HasColumnType("VARCHAR2(20)");
            });

            modelBuilder.Entity<DcDistrictEc>(entity =>
            {
                entity.HasKey(e => e.DistrictNumber)
                    .HasName("DC_DISTRICT_EC_PK");

                entity.ToTable("DC_DISTRICT_EC");

                entity.HasIndex(e => e.DistrictNumber)
                    .HasName("DC_DISTRICT_EC_PK")
                    .IsUnique();

                entity.Property(e => e.DistrictNumber)
                    .HasColumnName("DISTRICT_NUMBER")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.DistrictCode)
                    .IsRequired()
                    .HasColumnName("DISTRICT_CODE")
                    .HasColumnType("VARCHAR2(10)");

                entity.Property(e => e.DistrictName)
                    .IsRequired()
                    .HasColumnName("DISTRICT_NAME")
                    .HasColumnType("VARCHAR2(20)");
            });

            modelBuilder.Entity<DcDocumentType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK_DOCUMENT_TYPE");

                entity.ToTable("DC_DOCUMENT_TYPE");

                entity.HasIndex(e => e.TypeId)
                    .HasName("PK_DOCUMENT_TYPE")
                    .IsUnique();

                entity.Property(e => e.TypeId)
                    .HasColumnName("TYPE_ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.TypeName)
                    .HasColumnName("TYPE_NAME")
                    .HasColumnType("VARCHAR2(255)");
            });

            modelBuilder.Entity<DcExclusions>(entity =>
            {
                entity.HasKey(e => e.IdNo)
                    .HasName("DC_EXCLUSIONS_PK");

                entity.ToTable("DC_EXCLUSIONS");

                entity.HasIndex(e => e.IdNo)
                    .HasName("PRIMARYKEY")
                    .IsUnique();

                entity.Property(e => e.IdNo)
                    .HasColumnName("ID_NO")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.ExclDate)
                    .HasColumnName("EXCL_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.ExclusionBatchId)
                    .HasColumnName("EXCLUSION_BATCH_ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.ExclusionType)
                    .IsRequired()
                    .HasColumnName("EXCLUSION_TYPE")
                    .HasMaxLength(20);

                entity.Property(e => e.RegionId)
                    .HasColumnName("REGION_ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("USERNAME")
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<DcFile>(entity =>
            {
                entity.HasKey(e => e.UnqFileNo);

                entity.ToTable("DC_FILE");

                entity.HasIndex(e => e.ApplicantNo)
                    .HasName("INDEX7");

                entity.HasIndex(e => e.ApplicationStatus)
                    .HasName("INDEX26");

                entity.HasIndex(e => e.BrmBarcode)
                    .HasName("INDEX24");

                entity.HasIndex(e => e.ChildIdNo)
                    .HasName("INDEX41");

                entity.HasIndex(e => e.FileNumber)
                    .HasName("INDEX44");

                entity.HasIndex(e => e.FileStatus)
                    .HasName("INDEX27");

                entity.HasIndex(e => e.MisBoxDate)
                    .HasName("INDEX25");

                entity.HasIndex(e => e.MisBoxStatus)
                    .HasName("INDEX28");

                entity.HasIndex(e => e.MisBoxno)
                    .HasName("INDEX23");

                entity.HasIndex(e => e.Missing)
                    .HasName("INDEX30");

                entity.HasIndex(e => e.NonCompliant)
                    .HasName("INDEX31");

                entity.HasIndex(e => e.PrintOrder)
                    .HasName("INDEX42");

                entity.HasIndex(e => e.RegionId)
                    .HasName("INDEX43");

                entity.HasIndex(e => e.TdwBoxno)
                    .HasName("INDEX29");

                entity.HasIndex(e => e.UnqFileNo)
                    .HasName("PK_DC_FILE")
                    .IsUnique();

                entity.HasIndex(e => new { e.UnqFileNo, e.RegionId })
                    .HasName("INDEX1");

                entity.Property(e => e.UnqFileNo)
                    .HasColumnName("UNQ_FILE_NO")
                    .HasColumnType("VARCHAR2(20)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.AltBoxNo)
                    .HasColumnName("ALT_BOX_NO")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.ApplicantNo)
                    .HasColumnName("APPLICANT_NO")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.ApplicationStatus)
                    .HasColumnName("APPLICATION_STATUS")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.ArchiveYear)
                    .HasColumnName("ARCHIVE_YEAR")
                    .HasColumnType("VARCHAR2(4)");

                entity.Property(e => e.BatchAddDate)
                    .HasColumnName("BATCH_ADD_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.BatchNo)
                    .HasColumnName("BATCH_NO")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.BrmBarcode)
                    .HasColumnName("BRM_BARCODE")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.ChildIdNo)
                    .HasColumnName("CHILD_ID_NO")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.Compliant)
                    .HasColumnName("COMPLIANT")
                    .HasColumnType("VARCHAR2(2)");

                entity.Property(e => e.DocsPresent)
                    .HasColumnName("DOCS_PRESENT")
                    .HasColumnType("VARCHAR2(255)");

                entity.Property(e => e.DocsScanned)
                    .HasColumnName("DOCS_SCANNED")
                    .HasColumnType("VARCHAR2(255)");

                entity.Property(e => e.Exclusions)
                    .HasColumnName("EXCLUSIONS")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.FileComment)
                    .HasColumnName("FILE_COMMENT")
                    .HasColumnType("VARCHAR2(255)");

                entity.Property(e => e.FileNumber)
                    .HasColumnName("FILE_NUMBER")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.FileStatus)
                    .HasColumnName("FILE_STATUS")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.GrantType)
                    .HasColumnName("GRANT_TYPE")
                    .HasColumnType("VARCHAR2(2)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Isreview)
                    .HasColumnName("ISREVIEW")
                    .HasColumnType("VARCHAR2(2)");

                entity.Property(e => e.Lastreviewdate)
                    .HasColumnName("LASTREVIEWDATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.Lctype)
                    .HasColumnName("LCTYPE")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.MisBoxDate)
                    .HasColumnName("MIS_BOX_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.MisBoxStatus)
                    .HasColumnName("MIS_BOX_STATUS")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.MisBoxno)
                    .HasColumnName("MIS_BOXNO")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.MisReboxDate)
                    .HasColumnName("MIS_REBOX_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.MisReboxStatus)
                    .HasColumnName("MIS_REBOX_STATUS")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.Missing)
                    .HasColumnName("MISSING")
                    .HasColumnType("VARCHAR2(2)");

                entity.Property(e => e.NonCompliant)
                    .HasColumnName("NON_COMPLIANT")
                    .HasColumnType("VARCHAR2(2)");

                entity.Property(e => e.OfficeId)
                    .HasColumnName("OFFICE_ID")
                    .HasColumnType("VARCHAR2(10)");

                entity.Property(e => e.PrintOrder)
                    .HasColumnName("PRINT_ORDER")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.QcDate)
                    .HasColumnName("QC_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.QcUserFn)
                    .HasColumnName("QC_USER_FN")
                    .HasColumnType("VARCHAR2(255)");

                entity.Property(e => e.QcUserLn)
                    .HasColumnName("QC_USER_LN")
                    .HasColumnType("VARCHAR2(255)");

                entity.Property(e => e.RegionId)
                    .HasColumnName("REGION_ID")
                    .HasColumnType("VARCHAR2(10)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.RegionIdFrom)
                    .HasColumnName("REGION_ID_FROM")
                    .HasColumnType("VARCHAR2(10)");

                entity.Property(e => e.ScanDatetime)
                    .HasColumnName("SCAN_DATETIME")
                    .HasColumnType("DATE");

                entity.Property(e => e.SrdNo)
                    .HasColumnName("SRD_NO")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.TdwBoxArchiveYear)
                    .HasColumnName("TDW_BOX_ARCHIVE_YEAR")
                    .HasColumnType("VARCHAR2(4)");

                entity.Property(e => e.TdwBoxTypeId)
                    .HasColumnName("TDW_BOX_TYPE_ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.TdwBoxno)
                    .HasColumnName("TDW_BOXNO")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.TempBoxNo)
                    .HasColumnName("TEMP_BOX_NO")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.TransDate)
                    .HasColumnName("TRANS_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.TransType)
                    .HasColumnName("TRANS_TYPE")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Transferred)
                    .HasColumnName("TRANSFERRED")
                    .HasColumnType("VARCHAR2(2)");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("UPDATED_BY")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.UpdatedByAd)
                    .HasColumnName("UPDATED_BY_AD")
                    .HasColumnType("VARCHAR2(225)");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("UPDATED_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.UserFirstname)
                    .HasColumnName("USER_FIRSTNAME")
                    .HasColumnType("VARCHAR2(255)");

                entity.Property(e => e.UserLastname)
                    .HasColumnName("USER_LASTNAME")
                    .HasColumnType("VARCHAR2(255)");

                entity.HasOne(d => d.BatchNoNavigation)
                    .WithMany(p => p.DcFile)
                    .HasForeignKey(d => d.BatchNo)
                    .HasConstraintName("FK_DC_BATCH");

                entity.HasOne(d => d.GrantTypeNavigation)
                    .WithMany(p => p.DcFile)
                    .HasForeignKey(d => d.GrantType)
                    .HasConstraintName("FK_DC_FILE_GRANT");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.DcFile)
                    .HasForeignKey(d => d.OfficeId)
                    .HasConstraintName("FK_OFFICE_ID_FILE");

                entity.HasOne(d => d.TransTypeNavigation)
                    .WithMany(p => p.DcFile)
                    .HasForeignKey(d => d.TransType)
                    .HasConstraintName("FK_DC_FILE_TRANS_TYPE");
            });

            modelBuilder.Entity<DcFileRec>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DC_FILE_REC");

                entity.HasIndex(e => e.ApplicantNo)
                    .HasName("INDEX50");

                entity.HasIndex(e => e.ApplicationStatus)
                    .HasName("INDEX56");

                entity.HasIndex(e => e.BrmBarcode)
                    .HasName("INDEX54");

                entity.HasIndex(e => e.FileStatus)
                    .HasName("INDEX57");

                entity.HasIndex(e => e.MisBoxDate)
                    .HasName("INDEX55");

                entity.HasIndex(e => e.MisBoxStatus)
                    .HasName("INDEX58");

                entity.HasIndex(e => e.MisBoxno)
                    .HasName("INDEX53");

                entity.HasIndex(e => e.RegionId)
                    .HasName("INDEX51");

                entity.HasIndex(e => e.TdwBoxno)
                    .HasName("INDEX59");

                entity.HasIndex(e => e.UnqFileNo)
                    .HasName("INDEX52")
                    .IsUnique();

                entity.Property(e => e.AltBoxNo)
                    .HasColumnName("ALT_BOX_NO")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.ApplicantNo)
                    .HasColumnName("APPLICANT_NO")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.ApplicationStatus)
                    .HasColumnName("APPLICATION_STATUS")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.ArchiveYear)
                    .HasColumnName("ARCHIVE_YEAR")
                    .HasColumnType("VARCHAR2(4)");

                entity.Property(e => e.BatchAddDate)
                    .HasColumnName("BATCH_ADD_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.BatchNo)
                    .HasColumnName("BATCH_NO")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.BrmBarcode)
                    .HasColumnName("BRM_BARCODE")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.ChildIdNo)
                    .HasColumnName("CHILD_ID_NO")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.Compliant)
                    .HasColumnName("COMPLIANT")
                    .HasColumnType("VARCHAR2(2)");

                entity.Property(e => e.DocsPresent)
                    .HasColumnName("DOCS_PRESENT")
                    .HasColumnType("VARCHAR2(255)");

                entity.Property(e => e.DocsScanned)
                    .HasColumnName("DOCS_SCANNED")
                    .HasColumnType("VARCHAR2(255)");

                entity.Property(e => e.Exclusions)
                    .HasColumnName("EXCLUSIONS")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.FileComment)
                    .HasColumnName("FILE_COMMENT")
                    .HasColumnType("VARCHAR2(255)");

                entity.Property(e => e.FileNumber)
                    .HasColumnName("FILE_NUMBER")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.FileStatus)
                    .HasColumnName("FILE_STATUS")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.GrantType)
                    .HasColumnName("GRANT_TYPE")
                    .HasColumnType("VARCHAR2(2)");

                entity.Property(e => e.Isreview)
                    .HasColumnName("ISREVIEW")
                    .HasColumnType("VARCHAR2(2)");

                entity.Property(e => e.Lastreviewdate)
                    .HasColumnName("LASTREVIEWDATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.Lctype)
                    .HasColumnName("LCTYPE")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.MisBoxDate)
                    .HasColumnName("MIS_BOX_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.MisBoxStatus)
                    .HasColumnName("MIS_BOX_STATUS")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.MisBoxno)
                    .HasColumnName("MIS_BOXNO")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.MisReboxDate)
                    .HasColumnName("MIS_REBOX_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.MisReboxStatus)
                    .HasColumnName("MIS_REBOX_STATUS")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.Missing)
                    .HasColumnName("MISSING")
                    .HasColumnType("VARCHAR2(2)");

                entity.Property(e => e.NonCompliant)
                    .HasColumnName("NON_COMPLIANT")
                    .HasColumnType("VARCHAR2(2)");

                entity.Property(e => e.OfficeId)
                    .HasColumnName("OFFICE_ID")
                    .HasColumnType("VARCHAR2(10)");

                entity.Property(e => e.PrintOrder)
                    .HasColumnName("PRINT_ORDER")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.QcDate)
                    .HasColumnName("QC_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.QcUserFn)
                    .HasColumnName("QC_USER_FN")
                    .HasColumnType("VARCHAR2(255)");

                entity.Property(e => e.QcUserLn)
                    .HasColumnName("QC_USER_LN")
                    .HasColumnType("VARCHAR2(255)");

                entity.Property(e => e.RegionId)
                    .HasColumnName("REGION_ID")
                    .HasColumnType("VARCHAR2(10)");

                entity.Property(e => e.RegionIdFrom)
                    .HasColumnName("REGION_ID_FROM")
                    .HasColumnType("VARCHAR2(10)");

                entity.Property(e => e.ScanDatetime)
                    .HasColumnName("SCAN_DATETIME")
                    .HasColumnType("DATE");

                entity.Property(e => e.SrdNo)
                    .HasColumnName("SRD_NO")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.TdwBoxArchiveYear)
                    .HasColumnName("TDW_BOX_ARCHIVE_YEAR")
                    .HasColumnType("VARCHAR2(4)");

                entity.Property(e => e.TdwBoxTypeId)
                    .HasColumnName("TDW_BOX_TYPE_ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.TdwBoxno)
                    .HasColumnName("TDW_BOXNO")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.TempBoxNo)
                    .HasColumnName("TEMP_BOX_NO")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.TransDate)
                    .HasColumnName("TRANS_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.TransType)
                    .HasColumnName("TRANS_TYPE")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Transferred)
                    .HasColumnName("TRANSFERRED")
                    .HasColumnType("VARCHAR2(2)");

                entity.Property(e => e.UnqFileNo)
                    .HasColumnName("UNQ_FILE_NO")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("UPDATED_BY")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.UpdatedByAd)
                    .HasColumnName("UPDATED_BY_AD")
                    .HasColumnType("VARCHAR2(225)");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("UPDATED_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.UserFirstname)
                    .HasColumnName("USER_FIRSTNAME")
                    .HasColumnType("VARCHAR2(255)");

                entity.Property(e => e.UserLastname)
                    .HasColumnName("USER_LASTNAME")
                    .HasColumnType("VARCHAR2(255)");
            });

            modelBuilder.Entity<DcFileRequest>(entity =>
            {
                entity.HasKey(e => e.UnqFileNo);

                entity.ToTable("DC_FILE_REQUEST");

                entity.HasIndex(e => e.UnqFileNo)
                    .HasName("PK_DC_FILE_REQUEST")
                    .IsUnique();

                entity.Property(e => e.UnqFileNo)
                    .HasColumnName("UNQ_FILE_NO")
                    .HasColumnType("VARCHAR2(20)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.AppDate)
                    .HasColumnName("APP_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.ApplicationStatus)
                    .HasColumnName("APPLICATION_STATUS")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.BinId)
                    .HasColumnName("BIN_ID")
                    .HasColumnType("VARCHAR2(10)");

                entity.Property(e => e.BoxNumber)
                    .HasColumnName("BOX_NUMBER")
                    .HasColumnType("VARCHAR2(10)");

                entity.Property(e => e.BrmBarcode)
                    .HasColumnName("BRM_BARCODE")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.ClmNumber)
                    .HasColumnName("CLM_NUMBER")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.ClosedBy)
                    .HasColumnName("CLOSED_BY")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.ClosedByAd)
                    .HasColumnName("CLOSED_BY_AD")
                    .HasColumnType("VARCHAR2(225)");

                entity.Property(e => e.ClosedDate)
                    .HasColumnName("CLOSED_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.FileRetrieved)
                    .HasColumnName("FILE_RETRIEVED")
                    .HasColumnType("CHAR(1)");

                entity.Property(e => e.GrantType)
                    .HasColumnName("GRANT_TYPE")
                    .HasColumnType("VARCHAR2(2)");

                entity.Property(e => e.IdNo)
                    .HasColumnName("ID_NO")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.MisFileNo)
                    .HasColumnName("MIS_FILE_NO")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.PickedBy)
                    .HasColumnName("PICKED_BY")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.PicklistNo)
                    .HasColumnName("PICKLIST_NO")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.PicklistStatus)
                    .HasColumnName("PICKLIST_STATUS")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.PicklistType)
                    .HasColumnName("PICKLIST_TYPE")
                    .HasColumnType("VARCHAR2(10)");

                entity.Property(e => e.Position)
                    .HasColumnName("POSITION")
                    .HasColumnType("VARCHAR2(10)");

                entity.Property(e => e.ReceivedTdw)
                    .HasColumnName("RECEIVED_TDW")
                    .HasColumnType("CHAR(1)");

                entity.Property(e => e.RegionId)
                    .HasColumnName("REGION_ID")
                    .HasColumnType("VARCHAR2(10)");

                entity.Property(e => e.RegionIdTo)
                    .HasColumnName("REGION_ID_TO")
                    .HasColumnType("VARCHAR2(10)");

                entity.Property(e => e.RelatedMisFileNo)
                    .HasColumnName("RELATED_MIS_FILE_NO")
                    .HasColumnType("VARCHAR2(255)");

                entity.Property(e => e.ReqCategory)
                    .HasColumnName("REQ_CATEGORY")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.ReqCategoryDetail)
                    .HasColumnName("REQ_CATEGORY_DETAIL")
                    .HasColumnType("VARCHAR2(1000)");

                entity.Property(e => e.ReqCategoryType)
                    .HasColumnName("REQ_CATEGORY_TYPE")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.RequestedBy)
                    .HasColumnName("REQUESTED_BY")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.RequestedByAd)
                    .HasColumnName("REQUESTED_BY_AD")
                    .HasColumnType("VARCHAR2(225)");

                entity.Property(e => e.RequestedDate)
                    .HasColumnName("REQUESTED_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.RequestedOfficeId)
                    .HasColumnName("REQUESTED_OFFICE_ID")
                    .HasColumnType("VARCHAR2(10)");

                entity.Property(e => e.ScannedBy)
                    .HasColumnName("SCANNED_BY")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.ScannedByAd)
                    .HasColumnName("SCANNED_BY_AD")
                    .HasColumnType("VARCHAR2(225)");

                entity.Property(e => e.ScannedDate)
                    .HasColumnName("SCANNED_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.ScannedPhysicalInd)
                    .HasColumnName("SCANNED_PHYSICAL_IND")
                    .HasColumnType("CHAR(1)");

                entity.Property(e => e.SendToRequestor)
                    .HasColumnName("SEND_TO_REQUESTOR")
                    .HasColumnType("CHAR(1)");

                entity.Property(e => e.SentTdw)
                    .HasColumnName("SENT_TDW")
                    .HasColumnType("CHAR(1)");

                entity.Property(e => e.ServBy)
                    .HasColumnName("SERV_BY")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Stakeholder)
                    .HasColumnName("STAKEHOLDER")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Surname)
                    .HasColumnName("SURNAME")
                    .HasColumnType("VARCHAR2(200)");

                entity.Property(e => e.TdwBoxno)
                    .HasColumnName("TDW_BOXNO")
                    .HasColumnType("VARCHAR2(20)");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.DcFileRequest)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK_FILE_REQUEST_REGION_ID");

                entity.HasOne(d => d.StakeholderNavigation)
                    .WithMany(p => p.DcFileRequest)
                    .HasForeignKey(d => d.Stakeholder)
                    .HasConstraintName("DC_FILE_REQUEST_FK1");

                entity.HasOne(d => d.ReqCategoryNavigation)
                    .WithMany(p => p.DcFileRequest)
                    .HasForeignKey(d => new { d.ReqCategory, d.ReqCategoryType })
                    .HasConstraintName("FK_REQUEST_CATEGORY");
            });

            modelBuilder.Entity<DcGrantDocLink>(entity =>
            {
                entity.HasKey(e => new { e.GrantId, e.TransactionId, e.DocumentId })
                    .HasName("PK_GRAND_DOC_TRANS");

                entity.ToTable("DC_GRANT_DOC_LINK");

                entity.HasIndex(e => new { e.GrantId, e.TransactionId, e.DocumentId })
                    .HasName("PK_GRAND_DOC_TRANS")
                    .IsUnique();

                entity.Property(e => e.GrantId)
                    .HasColumnName("GRANT_ID")
                    .HasColumnType("VARCHAR2(2)");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("TRANSACTION_ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.DocumentId)
                    .HasColumnName("DOCUMENT_ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.CriticalFlag)
                    .HasColumnName("CRITICAL_FLAG")
                    .HasColumnType("CHAR(1)");

                entity.Property(e => e.Section)
                    .HasColumnName("SECTION")
                    .HasColumnType("VARCHAR2(100)");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.DcGrantDocLink)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DOCUMENT_TYPE_LINK");

                entity.HasOne(d => d.Grant)
                    .WithMany(p => p.DcGrantDocLink)
                    .HasForeignKey(d => d.GrantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GRANT_TYPE_GRANT");

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.DcGrantDocLink)
                    .HasForeignKey(d => d.TransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DC_TRANS_TYPE");
            });

            modelBuilder.Entity<DcGrantType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK_GRANT_TYPE");

                entity.ToTable("DC_GRANT_TYPE");

                entity.HasIndex(e => e.TypeId)
                    .HasName("PK_GRANT_TYPE")
                    .IsUnique();

                entity.Property(e => e.TypeId)
                    .HasColumnName("TYPE_ID")
                    .HasColumnType("VARCHAR2(2)");

                entity.Property(e => e.TypeName)
                    .HasColumnName("TYPE_NAME")
                    .HasColumnType("VARCHAR2(100)");
            });

            modelBuilder.Entity<DcLcType>(entity =>
            {
                entity.HasKey(e => e.Pk)
                    .HasName("DC_LC_TYPE_PK");

                entity.ToTable("DC_LC_TYPE");

                entity.HasIndex(e => e.Pk)
                    .HasName("DC_LC_TYPE_PK")
                    .IsUnique();

                entity.Property(e => e.Pk)
                    .HasColumnName("PK")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("DESCRIPTION")
                    .HasColumnType("VARCHAR2(100)");
            });

            modelBuilder.Entity<DcLocalOffice>(entity =>
            {
                entity.HasKey(e => e.OfficeId);

                entity.ToTable("DC_LOCAL_OFFICE");

                entity.HasIndex(e => e.District)
                    .HasName("INDEX48");

                entity.HasIndex(e => e.OfficeId)
                    .HasName("PK_DC_LOCAL_OFFICE")
                    .IsUnique();

                entity.HasIndex(e => e.OfficeName)
                    .HasName("INDEX45");

                entity.HasIndex(e => e.OfficeType)
                    .HasName("INDEX47");

                entity.HasIndex(e => e.RegionId)
                    .HasName("INDEX46");

                entity.Property(e => e.OfficeId)
                    .HasColumnName("OFFICE_ID")
                    .HasColumnType("VARCHAR2(10)");

                entity.Property(e => e.District)
                    .HasColumnName("DISTRICT")
                    .HasColumnType("VARCHAR2(255)");

                entity.Property(e => e.OfficeName)
                    .HasColumnName("OFFICE_NAME")
                    .HasColumnType("VARCHAR2(255)");

                entity.Property(e => e.OfficeType)
                    .HasColumnName("OFFICE_TYPE")
                    .HasColumnType("VARCHAR2(3)");

                entity.Property(e => e.RegionId)
                    .HasColumnName("REGION_ID")
                    .HasColumnType("VARCHAR2(20)");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.DcLocalOffice)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK_LOCAL_OFFICE_REGION");
            });

            modelBuilder.Entity<DcMerge>(entity =>
            {
                entity.HasKey(e => e.Pk)
                    .HasName("DC_MERGE_PK");

                entity.ToTable("DC_MERGE");

                entity.HasIndex(e => e.Pk)
                    .HasName("DC_MERGE_PK")
                    .IsUnique();

                entity.Property(e => e.Pk)
                    .HasColumnName("PK")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.BrmBarcode)
                    .IsRequired()
                    .HasColumnName("BRM_BARCODE")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.ParentBrmBarcode)
                    .IsRequired()
                    .HasColumnName("PARENT_BRM_BARCODE")
                    .HasColumnType("VARCHAR2(20)");
            });

            modelBuilder.Entity<DcOfficeKuafLink>(entity =>
            {
                entity.HasKey(e => e.Pk)
                    .HasName("DC_OFFICE_KUAF_LINK_PK");

                entity.ToTable("DC_OFFICE_KUAF_LINK");

                entity.HasIndex(e => e.Pk)
                    .HasName("DC_OFFICE_KUAF_LINK_PK")
                    .IsUnique();

                entity.Property(e => e.Pk)
                    .HasColumnName("PK")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.KuafId)
                    .HasColumnName("KUAF_ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.OfficeId)
                    .HasColumnName("OFFICE_ID")
                    .HasColumnType("VARCHAR2(10)");

                entity.Property(e => e.Supervisor)
                    .HasColumnName("SUPERVISOR")
                    .HasColumnType("VARCHAR2(10)");

                entity.Property(e => e.Username)
                    .HasColumnName("USERNAME")
                    .HasColumnType("VARCHAR2(225)");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.DcOfficeKuafLink)
                    .HasForeignKey(d => d.OfficeId)
                    .HasConstraintName("FK_LOCAL_OFFICE");
            });

            modelBuilder.Entity<DcPicklist>(entity =>
            {
                entity.HasKey(e => e.UnqPicklist)
                    .HasName("DC_PICKLIST_PK");

                entity.ToTable("DC_PICKLIST");

                entity.HasIndex(e => e.UnqPicklist)
                    .HasName("DC_PICKLIST_PK")
                    .IsUnique();

                entity.Property(e => e.UnqPicklist)
                    .HasColumnName("UNQ_PICKLIST")
                    .HasColumnType("VARCHAR2(20)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.PicklistDate)
                    .HasColumnName("PICKLIST_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.PicklistStatus)
                    .HasColumnName("PICKLIST_STATUS")
                    .HasColumnType("CHAR(1)");

                entity.Property(e => e.RegionId)
                    .IsRequired()
                    .HasColumnName("REGION_ID")
                    .HasColumnType("VARCHAR2(10)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.RegistryType)
                    .IsRequired()
                    .HasColumnName("REGISTRY_TYPE")
                    .HasColumnType("CHAR(1)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Userid)
                    .HasColumnName("USERID")
                    .HasColumnType("NUMBER");
            });

            modelBuilder.Entity<DcRegion>(entity =>
            {
                entity.HasKey(e => e.RegionId);

                entity.ToTable("DC_REGION");

                entity.HasIndex(e => e.RegionId)
                    .HasName("PK_DC_REGION")
                    .IsUnique();

                entity.HasIndex(e => new { e.RegionName, e.RegionCode, e.RegionId })
                    .HasName("INDEX40");

                entity.Property(e => e.RegionId)
                    .HasColumnName("REGION_ID")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.RegionCode)
                    .HasColumnName("REGION_CODE")
                    .HasColumnType("VARCHAR2(10)");

                entity.Property(e => e.RegionName)
                    .HasColumnName("REGION_NAME")
                    .HasColumnType("VARCHAR2(100)");
            });

            modelBuilder.Entity<DcReqCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK_REQ_CATEGORY");

                entity.ToTable("DC_REQ_CATEGORY");

                entity.HasIndex(e => e.CategoryId)
                    .HasName("PK_REQ_CATEGORY")
                    .IsUnique();

                entity.Property(e => e.CategoryId)
                    .HasColumnName("CATEGORY_ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.CategoryDescr)
                    .HasColumnName("CATEGORY_DESCR")
                    .HasColumnType("VARCHAR2(255)");
            });

            modelBuilder.Entity<DcReqCategoryType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK_CATEGORY_TYPE");

                entity.ToTable("DC_REQ_CATEGORY_TYPE");

                entity.HasIndex(e => e.TypeId)
                    .HasName("PK_CATEGORY_TYPE")
                    .IsUnique();

                entity.Property(e => e.TypeId)
                    .HasColumnName("TYPE_ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.TypeDescr)
                    .HasColumnName("TYPE_DESCR")
                    .HasColumnType("VARCHAR2(255)");
            });

            modelBuilder.Entity<DcReqCategoryTypeLink>(entity =>
            {
                entity.HasKey(e => new { e.CategoryId, e.TypeId })
                    .HasName("PK_CATEGORY_TYPE_LINK");

                entity.ToTable("DC_REQ_CATEGORY_TYPE_LINK");

                entity.HasIndex(e => new { e.CategoryId, e.TypeId })
                    .HasName("PK_CATEGORY_TYPE_LINK")
                    .IsUnique();

                entity.Property(e => e.CategoryId)
                    .HasColumnName("CATEGORY_ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.TypeId)
                    .HasColumnName("TYPE_ID")
                    .HasColumnType("NUMBER");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.DcReqCategoryTypeLink)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CATEGORY");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.DcReqCategoryTypeLink)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CATEGORY_TYPE");
            });

            modelBuilder.Entity<DcStakeholder>(entity =>
            {
                entity.HasKey(e => e.StakeholderId)
                    .HasName("DC_STAKEHOLDER_PK");

                entity.ToTable("DC_STAKEHOLDER");

                entity.HasIndex(e => e.StakeholderId)
                    .HasName("DC_STAKEHOLDER_PK")
                    .IsUnique();

                entity.Property(e => e.StakeholderId)
                    .HasColumnName("STAKEHOLDER_ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.DepartmentId)
                    .HasColumnName("DEPARTMENT_ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("EMAIL")
                    .HasColumnType("VARCHAR2(225)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.RegionId)
                    .HasColumnName("REGION_ID")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasColumnName("SURNAME")
                    .HasColumnType("VARCHAR2(64)");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.DcStakeholder)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DC_STAKEHOLDER_FK1");
            });

            modelBuilder.Entity<DcTransactionType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK_TRANSACTION_TYPE");

                entity.ToTable("DC_TRANSACTION_TYPE");

                entity.HasIndex(e => e.TypeId)
                    .HasName("PK_TRANSACTION_TYPE")
                    .IsUnique();

                entity.Property(e => e.TypeId)
                    .HasColumnName("TYPE_ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.ServiceCategory)
                    .HasColumnName("SERVICE_CATEGORY")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.TypeName)
                    .HasColumnName("TYPE_NAME")
                    .HasColumnType("VARCHAR2(255)");
            });

            modelBuilder.HasSequence("ACTIVEVIEWOVERRIDESSEQUENCE");

            modelBuilder.HasSequence("AGENTSEQUENCE");

            modelBuilder.HasSequence("AUDITCOLLECTIONSITEMSSEQ");

            modelBuilder.HasSequence("CUST_DISTRICTSEQ");

            modelBuilder.HasSequence("DAUDITNEWSEQUENCE");

            modelBuilder.HasSequence("DDOCUMENTCLASSSEQUENCE");

            modelBuilder.HasSequence("DFAVORITESTABSSEQUENCE");

            modelBuilder.HasSequence("DPSINSRTPROPSSEQ");

            modelBuilder.HasSequence("DPSTASKSSEQUENCE");

            modelBuilder.HasSequence("DSOCIALFEEDEVENTSSEQ");

            modelBuilder.HasSequence("DSOCIALFOLLOWERSSEQ");

            modelBuilder.HasSequence("DSTAGINGIMPORTSEQUENCE");

            modelBuilder.HasSequence("DSUGGESTWORDSPENDINGSEQUENCE");

            modelBuilder.HasSequence("DSUGGESTWORDSSEQUENCE");

            modelBuilder.HasSequence("DTREEASPECTSNOTIFYSEQUENCE");

            modelBuilder.HasSequence("DTREECOREEXTSOURCESEQUENCE");

            modelBuilder.HasSequence("DTREENOTIFYSEQUENCE");

            modelBuilder.HasSequence("ELINKMESSAGESEQUENCE");

            modelBuilder.HasSequence("FILECACHESEQUENCE");

            modelBuilder.HasSequence("ISEQ$$_169018");

            modelBuilder.HasSequence("ISEQ$$_169042");

            modelBuilder.HasSequence("ISEQ$$_169110");

            modelBuilder.HasSequence("ISEQ$$_169251");

            modelBuilder.HasSequence("ISEQ$$_171021");

            modelBuilder.HasSequence("ISEQ$$_171023");

            modelBuilder.HasSequence("ISEQ$$_171027");

            modelBuilder.HasSequence("ISEQ$$_171030");

            modelBuilder.HasSequence("ISEQ$$_171033");

            modelBuilder.HasSequence("ISEQ$$_171036");

            modelBuilder.HasSequence("ISEQ$$_171039");

            modelBuilder.HasSequence("ISEQ$$_171042");

            modelBuilder.HasSequence("ISEQ$$_171045");

            modelBuilder.HasSequence("KUAFIDENTITYSEQUENCE");

            modelBuilder.HasSequence("KUAFIDENTITYTYPESEQUENCE");

            modelBuilder.HasSequence("LLEVENTSSEQUENCE");

            modelBuilder.HasSequence("NOTIFYSEQUENCE");

            modelBuilder.HasSequence("OI_STATUS_SEQ");

            modelBuilder.HasSequence("PROVIDERRETRYSEQUENCE");

            modelBuilder.HasSequence("RECD_HOTSEQ");

            modelBuilder.HasSequence("RECD_OPERATIONTRACKINGSEQ");

            modelBuilder.HasSequence("RENDITIONFOLDERSSEQ");

            modelBuilder.HasSequence("RENDITIONMIMETYPERULESSEQ");

            modelBuilder.HasSequence("RENDITIONNODERULESSEQ");

            modelBuilder.HasSequence("RENDITIONQUEUESEQ");

            modelBuilder.HasSequence("RESULTIDSEQUENCE");

            modelBuilder.HasSequence("RETENTIONUPDATEFAILEDSEQNID");

            modelBuilder.HasSequence("RETENTIONUPDATELOGSEQNID");

            modelBuilder.HasSequence("RETENTIONUPDATEORDERSEQNID");

            modelBuilder.HasSequence("RM_HOLDQUERYHISTORYSEQUENCE");

            modelBuilder.HasSequence("RMSEC_DEFINEDRULESEQUENCE");

            modelBuilder.HasSequence("SEARCHSTATSSEQUENCE");

            modelBuilder.HasSequence("SEQ_CUST_REGION_REGNUM");

            modelBuilder.HasSequence("SEQ_DC_ALT_BOX_NO_ECA");

            modelBuilder.HasSequence("SEQ_DC_ALT_BOX_NO_FST");

            modelBuilder.HasSequence("SEQ_DC_ALT_BOX_NO_GAU");

            modelBuilder.HasSequence("SEQ_DC_ALT_BOX_NO_KZN");

            modelBuilder.HasSequence("SEQ_DC_ALT_BOX_NO_LIM");

            modelBuilder.HasSequence("SEQ_DC_ALT_BOX_NO_MPU");

            modelBuilder.HasSequence("SEQ_DC_ALT_BOX_NO_NCA");

            modelBuilder.HasSequence("SEQ_DC_ALT_BOX_NO_NWP");

            modelBuilder.HasSequence("SEQ_DC_ALT_BOX_NO_WCA");

            modelBuilder.HasSequence("SEQ_DC_BATCH");

            modelBuilder.HasSequence("SEQ_DC_BOXPICKED");

            modelBuilder.HasSequence("SEQ_DC_FILE");

            modelBuilder.HasSequence("SEQ_DC_FILE_REQUEST");

            modelBuilder.HasSequence("SEQ_DC_PICKLIST");

            modelBuilder.HasSequence("WORKERQUEUESEQUENCE");

            modelBuilder.HasSequence("WWORKAUDITSEQ");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
