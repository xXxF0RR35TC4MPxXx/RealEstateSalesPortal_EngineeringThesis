using Inżynierka.Shared.Entities;
using Inżynierka.Shared.Entities.OfferTypes.Apartment;
using Inżynierka.Shared.Entities.OfferTypes.BusinessPremises;
using Inżynierka.Shared.Entities.OfferTypes.Garage;
using Inżynierka.Shared.Entities.OfferTypes.Hall;
using Inżynierka.Shared.Entities.OfferTypes.House;
using Inżynierka.Shared.Entities.OfferTypes.Plot;
using Inżynierka.Shared.Entities.OfferTypes.Room;
using Microsoft.EntityFrameworkCore;

namespace Inżynierka.Shared
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        public DbSet<UserAction> UserActions { get; set; }
        public DbSet<UserFavourite> UserFavourites { get; set; }
        public DbSet<UserMessage> UserMessages { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<RoomRentingOffer> RoomRentingOffers { get; set; }
        public DbSet<PlotOffer> PlotOffers { get; set; }
        public DbSet<HouseRentOffer> HouseRentOffers { get; set; }
        public DbSet<HouseSaleOffer> HouseSaleOffers { get; set; }
        public DbSet<HallRentOffer> HallRentOffers { get; set; }
        public DbSet<HallSaleOffer> HallSaleOffers { get; set; }
        public DbSet<BusinessPremisesRentOffer> BusinessPremisesRentOffers { get; set; }
        public DbSet<BusinessPremisesSaleOffer> BusinessPremisesSaleOffers { get; set; }
        public DbSet<GarageRentOffer> GarageRentOffers { get; set; }
        public DbSet<GarageSaleOffer> GarageSaleOffers { get; set; }
        public DbSet<ApartmentRentOffer> ApartmentRentOffers { get; set; }
        public DbSet<ApartmentSaleOffer> ApartmentSaleOffers { get; set; }
        public DbSet<UserPreferenceForm> UserPreferenceForms { get; set; }
        public DbSet<SingleFormProposal> SingleFormProposals { get; set; }
        public DbSet<OfferVisibleForUser> OfferVisibleForUsers { get; set; }
        public DbSet<Log> Logs { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserFavourite>().HasKey(ul => new
            {
                ul.OfferId,
                ul.UserId
            });

            modelBuilder.Entity<SingleFormProposal>().HasKey(ul => new
            {
                ul.OfferId,
                ul.FormId
            });
            
            modelBuilder.Entity<OfferVisibleForUser>().HasKey(ul => new
            {
                ul.OfferId,
                ul.UserId
            });

            modelBuilder.Entity<Agency>().HasMany(agency => agency.Agents).WithOne(a => a.AgentInAgency).HasForeignKey(agent => agent.AgentInAgencyId);
            modelBuilder.Entity<Agency>().HasOne(agency => agency.Owner).WithOne(a => a.OwnerOfAgency).HasForeignKey<User>(user => user.OwnerOfAgencyId);
            //to co wyżej tylko dla formularza i single form proposal?

            modelBuilder.Entity<RoomRentingOffer>().ToTable("RoomRentingOffer");
            modelBuilder.Entity<PlotOffer>().ToTable("PlotOffer");
            modelBuilder.Entity<HouseRentOffer>().ToTable("HouseRentOffer");
            modelBuilder.Entity<HouseSaleOffer>().ToTable("HouseSaleOffer");
            modelBuilder.Entity<HallRentOffer>().ToTable("HallRentOffer");
            modelBuilder.Entity<HallSaleOffer>().ToTable("HallSaleOffer");
            modelBuilder.Entity<BusinessPremisesRentOffer>().ToTable("BusinessPremisesRentOffer");
            modelBuilder.Entity<BusinessPremisesSaleOffer>().ToTable("BusinessPremisesSaleOffer");
            modelBuilder.Entity<GarageRentOffer>().ToTable("GarageRentOffer");
            modelBuilder.Entity<GarageSaleOffer>().ToTable("GarageSaleOffer");
            modelBuilder.Entity<ApartmentRentOffer>().ToTable("ApartmentRentOffer");
            modelBuilder.Entity<ApartmentSaleOffer>().ToTable("ApartmentSaleOffer");
            //modelBuilder.Entity<Offer>().UseTpcMappingStrategy();
        }

        public void AttachEntity<TEntity>(TEntity entity) where TEntity : class, new()
        {
            Set<TEntity>().Attach(entity);
        }

        public void AddEntity<TEntity>(TEntity entity) where TEntity : class, new()
        {
            Set<TEntity>().Add(entity);
        }

        public void AddEntityAndSaveChanges<TEntity>(TEntity entity) where TEntity : class, new()
        {
            AddEntity(entity);
            SaveChanges();
        }

        public void AddEntitiesRange<TEntity>(IEnumerable<TEntity> entity) where TEntity : class, new()
        {
            Set<TEntity>().AddRange(entity);
        }

        public void AddEntitiesRangeAndSaveChanges<TEntity>(IEnumerable<TEntity> entity) where TEntity : class, new()
        {
            AddEntitiesRange(entity);
            SaveChanges();
        }

        public void UpdateEntity<TEntity>(TEntity entity) where TEntity : class, new()
        {
            Entry(entity).State = EntityState.Modified;
        }

        public void UpdateEntityAndSaveChanges<TEntity>(TEntity entity) where TEntity : class, new()
        {
            UpdateEntity(entity);
            SaveChanges();
        }

        public void UpdateEntitiesRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            foreach (var entity in entities)
            {
                UpdateEntity(entity);
            }
        }

        public void UpdateEntitiesRangeAndSaveChanges<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            UpdateEntitiesRange(entities);
            SaveChanges();
        }

        public void RemoveEntity<TEntity>(TEntity entity) where TEntity : class, new()
        {
            Set<TEntity>().Remove(entity);
        }

        public void RemoveEntitiesRange<TEntity>(IEnumerable<TEntity> entity) where TEntity : class, new()
        {
            Set<TEntity>().RemoveRange(entity);
        }

        public void RemoveEntitiesRangeAndSaveChanges<TEntity>(IEnumerable<TEntity> entity) where TEntity : class, new()
        {
            RemoveEntitiesRange(entity);
            SaveChanges();
        }
    }
}
