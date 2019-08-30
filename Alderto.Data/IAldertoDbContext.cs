using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Data.Models;
using Alderto.Data.Models.GuildBank;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Alderto.Data
{
    public interface IAldertoDbContext
    {
        // === 

        /// <summary>
        /// Guilds Table. Primary key: [<see cref="Guild.Id"/>]
        /// </summary>
        DbSet<Guild> Guilds { get; }

        /// <summary>
        /// Guild Members table. Primary key: [<see cref="GuildMember.MemberId"/>, <see cref="GuildMember.GuildId"/>]
        /// Joint table of Guilds and Members.
        /// </summary> 
        DbSet<GuildMember> GuildMembers { get; }

        /// <summary>
        /// Members table. Primary key: [<see cref="Member.Id"/>]
        /// </summary>
        DbSet<Member> Members { get; }

        // ===

        /// <summary>
        /// Guild's custom commands table. Primary key: [<see cref="CustomCommand.TriggerKeyword"/>, <see cref="CustomCommand.GuildId"/>]
        /// </summary>
        DbSet<CustomCommand> CustomCommands { get; }

        // ===

        /// <summary>
        /// Guild preferences. 1-to-0..1 relation with Guilds. Primary key: [<see cref="GuildConfiguration.GuildId"/>]
        /// </summary>
        DbSet<GuildConfiguration> GuildPreferences { get; }

        // ===

        DbSet<GuildBank> GuildBanks { get; }
        DbSet<GuildBankItem> GuildBankItems { get; }


        /// <summary>
        ///     Provides access to database related information and operations for this context.
        /// </summary>
        DatabaseFacade Database { get; }

        /// <summary>
        ///     Provides access to information and operations for entity instances this context is tracking.
        /// </summary>
        ChangeTracker ChangeTracker { get; }

        /// <summary>
        ///     The metadata about the shape of entities, the relationships between them, and how they map to the database.
        /// </summary>
        IModel Model { [DebuggerStepThrough] get; }

        /// <summary>
        ///     Creates a <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> that can be used to query and save instances of <typeparamref name="TEntity" />.
        /// </summary>
        /// <typeparam name="TEntity"> The type of entity for which a set should be returned. </typeparam>
        /// <returns> A set for the given entity type. </returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        ///     Creates a <see cref="T:Microsoft.EntityFrameworkCore.DbQuery`1" /> that can be used to query instances of <typeparamref name="TQuery" />.
        /// </summary>
        /// <typeparam name="TQuery"> The type of query for which a DbQuery should be returned. </typeparam>
        /// <returns> A DbQuery for the given query type. </returns>
        DbQuery<TQuery> Query<TQuery>() where TQuery : class;

        /// <summary>
        ///     Saves all changes made in this context to the database.
        /// </summary>
        /// <remarks>
        ///     This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" /> to discover any
        ///     changes to entity instances before saving to the underlying database. This can be disabled via
        ///     <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        /// </remarks>
        /// <returns>
        ///     The number of state entries written to the database.
        /// </returns>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateException">
        ///     An error is encountered while saving to the database.
        /// </exception>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException">
        ///     A concurrency violation is encountered while saving to the database.
        ///     A concurrency violation occurs when an unexpected number of rows are affected during save.
        ///     This is usually because the data in the database has been modified since it was loaded into memory.
        /// </exception>
        int SaveChanges();

        /// <summary>
        ///     Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">
        ///     Indicates whether <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AcceptAllChanges" /> is called after the changes have
        ///     been sent successfully to the database.
        /// </param>
        /// <remarks>
        ///     This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" /> to discover any
        ///     changes to entity instances before saving to the underlying database. This can be disabled via
        ///     <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        /// </remarks>
        /// <returns>
        ///     The number of state entries written to the database.
        /// </returns>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateException">
        ///     An error is encountered while saving to the database.
        /// </exception>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException">
        ///     A concurrency violation is encountered while saving to the database.
        ///     A concurrency violation occurs when an unexpected number of rows are affected during save.
        ///     This is usually because the data in the database has been modified since it was loaded into memory.
        /// </exception>
        int SaveChanges(bool acceptAllChangesOnSuccess);

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the database.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" /> to discover any
        ///         changes to entity instances before saving to the underlying database. This can be disabled via
        ///         <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        ///     </para>
        ///     <para>
        ///         Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        ///         that any asynchronous operations have completed before calling another method on this context.
        ///     </para>
        /// </remarks>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        ///     A task that represents the asynchronous save operation. The task result contains the
        ///     number of state entries written to the database.
        /// </returns>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateException">
        ///     An error is encountered while saving to the database.
        /// </exception>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException">
        ///     A concurrency violation is encountered while saving to the database.
        ///     A concurrency violation occurs when an unexpected number of rows are affected during save.
        ///     This is usually because the data in the database has been modified since it was loaded into memory.
        /// </exception>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the database.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">
        ///     Indicates whether <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AcceptAllChanges" /> is called after the changes have
        ///     been sent successfully to the database.
        /// </param>
        /// <remarks>
        ///     <para>
        ///         This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" /> to discover any
        ///         changes to entity instances before saving to the underlying database. This can be disabled via
        ///         <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        ///     </para>
        ///     <para>
        ///         Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        ///         that any asynchronous operations have completed before calling another method on this context.
        ///     </para>
        /// </remarks>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        ///     A task that represents the asynchronous save operation. The task result contains the
        ///     number of state entries written to the database.
        /// </returns>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateException">
        ///     An error is encountered while saving to the database.
        /// </exception>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException">
        ///     A concurrency violation is encountered while saving to the database.
        ///     A concurrency violation occurs when an unexpected number of rows are affected during save.
        ///     This is usually because the data in the database has been modified since it was loaded into memory.
        /// </exception>
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Releases the allocated resources for this context.
        /// </summary>
        void Dispose();

        /// <summary>
        ///     Gets an <see cref="T:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry`1" /> for the given entity. The entry provides
        ///     access to change tracking information and operations for the entity.
        /// </summary>
        /// <typeparam name="TEntity"> The type of the entity. </typeparam>
        /// <param name="entity"> The entity to get the entry for. </param>
        /// <returns> The entry for the given entity. </returns>
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        ///     <para>
        ///         Gets an <see cref="T:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry" /> for the given entity. The entry provides
        ///         access to change tracking information and operations for the entity.
        ///     </para>
        ///     <para>
        ///         This method may be called on an entity that is not tracked. You can then
        ///         set the <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State" /> property on the returned entry
        ///         to have the context begin tracking the entity in the specified state.
        ///     </para>
        /// </summary>
        /// <param name="entity"> The entity to get the entry for. </param>
        /// <returns> The entry for the given entity. </returns>
        EntityEntry Entry(object entity);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity, and any other reachable entities that are
        ///         not already being tracked, in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Added" /> state such that
        ///         they will be inserted into the database when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" /> is called.
        ///     </para>
        ///     <para>
        ///         Use <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State" /> to set the state of only a single entity.
        ///     </para>
        /// </summary>
        /// <typeparam name="TEntity"> The type of the entity. </typeparam>
        /// <param name="entity"> The entity to add. </param>
        /// <returns>
        ///     The <see cref="T:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry`1" /> for the entity. The entry provides
        ///     access to change tracking information and operations for the entity.
        /// </returns>
        EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity, and any other reachable entities that are
        ///         not already being tracked, in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Added" /> state such that they will
        ///         be inserted into the database when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" /> is called.
        ///     </para>
        ///     <para>
        ///         This method is async only to allow special value generators, such as the one used by
        ///         'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
        ///         to access the database asynchronously. For all other cases the non async method should be used.
        ///     </para>
        /// </summary>
        /// <typeparam name="TEntity"> The type of the entity. </typeparam>
        /// <param name="entity"> The entity to add. </param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        ///     A task that represents the asynchronous Add operation. The task result contains the
        ///     <see cref="T:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry`1" /> for the entity. The entry provides access to change tracking
        ///     information and operations for the entity.
        /// </returns>
        Task<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Unchanged" /> state
        ///         such that no operation will be performed when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" />
        ///         is called.
        ///     </para>
        ///     <para>
        ///         A recursive search of the navigation properties will be performed to find reachable entities
        ///         that are not already being tracked by the context. These entities will also begin to be tracked
        ///         by the context. If a reachable entity has its primary key value set
        ///         then it will be tracked in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Unchanged" /> state. If the primary key
        ///         value is not set then it will be tracked in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Added" /> state.
        ///         An entity is considered to have its primary key value set if the primary key property is set
        ///         to anything other than the CLR default for the property type.
        ///     </para>
        ///     <para>
        ///         Use <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State" /> to set the state of only a single entity.
        ///     </para>
        /// </summary>
        /// <typeparam name="TEntity"> The type of the entity. </typeparam>
        /// <param name="entity"> The entity to attach. </param>
        /// <returns>
        ///     The <see cref="T:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry`1" /> for the entity. The entry provides
        ///     access to change tracking information and operations for the entity.
        /// </returns>
        EntityEntry<TEntity> Attach<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Modified" /> state such that it will
        ///         be updated in the database when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" /> is called.
        ///     </para>
        ///     <para>
        ///         All properties of the entity will be marked as modified. To mark only some properties as modified, use
        ///         <see cref="M:Microsoft.EntityFrameworkCore.DbContext.Attach``1(``0)" /> to begin tracking the entity in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Unchanged" />
        ///         state and then use the returned <see cref="T:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry" /> to mark the desired properties as modified.
        ///     </para>
        ///     <para>
        ///         A recursive search of the navigation properties will be performed to find reachable entities
        ///         that are not already being tracked by the context. These entities will also begin to be tracked
        ///         by the context. If a reachable entity has its primary key value set
        ///         then it will be tracked in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Modified" /> state. If the primary key
        ///         value is not set then it will be tracked in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Added" /> state.
        ///         An entity is considered to have its primary key value set if the primary key property is set
        ///         to anything other than the CLR default for the property type.
        ///     </para>
        ///     <para>
        ///         Use <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State" /> to set the state of only a single entity.
        ///     </para>
        /// </summary>
        /// <typeparam name="TEntity"> The type of the entity. </typeparam>
        /// <param name="entity"> The entity to update. </param>
        /// <returns>
        ///     The <see cref="T:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry`1" /> for the entity. The entry provides
        ///     access to change tracking information and operations for the entity.
        /// </returns>
        EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        ///     Begins tracking the given entity in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Deleted" /> state such that it will
        ///     be removed from the database when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" /> is called.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         If the entity is already tracked in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Added" /> state then the context will
        ///         stop tracking the entity (rather than marking it as <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Deleted" />) since the
        ///         entity was previously added to the context and does not exist in the database.
        ///     </para>
        ///     <para>
        ///         Any other reachable entities that are not already being tracked will be tracked in the same way that
        ///         they would be if <see cref="M:Microsoft.EntityFrameworkCore.DbContext.Attach``1(``0)" /> was called before calling this method.
        ///         This allows any cascading actions to be applied when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" /> is called.
        ///     </para>
        ///     <para>
        ///         Use <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State" /> to set the state of only a single entity.
        ///     </para>
        /// </remarks>
        /// <typeparam name="TEntity"> The type of the entity. </typeparam>
        /// <param name="entity"> The entity to remove. </param>
        /// <returns>
        ///     The <see cref="T:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry`1" /> for the entity. The entry provides
        ///     access to change tracking information and operations for the entity.
        /// </returns>
        EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity, and any other reachable entities that are
        ///         not already being tracked, in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Added" /> state such that they will
        ///         be inserted into the database when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" /> is called.
        ///     </para>
        ///     <para>
        ///         Use <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State" /> to set the state of only a single entity.
        ///     </para>
        /// </summary>
        /// <param name="entity"> The entity to add. </param>
        /// <returns>
        ///     The <see cref="T:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry" /> for the entity. The entry provides
        ///     access to change tracking information and operations for the entity.
        /// </returns>
        EntityEntry Add(object entity);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity, and any other reachable entities that are
        ///         not already being tracked, in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Added" /> state such that they will
        ///         be inserted into the database when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" /> is called.
        ///     </para>
        ///     <para>
        ///         Use <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State" /> to set the state of only a single entity.
        ///     </para>
        ///     <para>
        ///         This method is async only to allow special value generators, such as the one used by
        ///         'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
        ///         to access the database asynchronously. For all other cases the non async method should be used.
        ///     </para>
        /// </summary>
        /// <param name="entity"> The entity to add. </param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        ///     A task that represents the asynchronous Add operation. The task result contains the
        ///     <see cref="T:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry" /> for the entity. The entry provides access to change tracking
        ///     information and operations for the entity.
        /// </returns>
        Task<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Unchanged" /> state
        ///         such that no operation will be performed when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" />
        ///         is called.
        ///     </para>
        ///     <para>
        ///         A recursive search of the navigation properties will be performed to find reachable entities
        ///         that are not already being tracked by the context. These entities will also begin to be tracked
        ///         by the context. If a reachable entity has its primary key value set
        ///         then it will be tracked in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Unchanged" /> state. If the primary key
        ///         value is not set then it will be tracked in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Added" /> state.
        ///         An entity is considered to have its primary key value set if the primary key property is set
        ///         to anything other than the CLR default for the property type.
        ///     </para>
        ///     <para>
        ///         Use <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State" /> to set the state of only a single entity.
        ///     </para>
        /// </summary>
        /// <param name="entity"> The entity to attach. </param>
        /// <returns>
        ///     The <see cref="T:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry" /> for the entity. The entry provides
        ///     access to change tracking information and operations for the entity.
        /// </returns>
        EntityEntry Attach(object entity);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Modified" /> state such that it will
        ///         be updated in the database when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" /> is called.
        ///     </para>
        ///     <para>
        ///         All properties of the entity will be marked as modified. To mark only some properties as modified, use
        ///         <see cref="M:Microsoft.EntityFrameworkCore.DbContext.Attach(System.Object)" /> to begin tracking the entity in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Unchanged" />
        ///         state and then use the returned <see cref="T:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry" /> to mark the desired properties as modified.
        ///     </para>
        ///     <para>
        ///         A recursive search of the navigation properties will be performed to find reachable entities
        ///         that are not already being tracked by the context. These entities will also begin to be tracked
        ///         by the context. If a reachable entity has its primary key value set
        ///         then it will be tracked in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Modified" /> state. If the primary key
        ///         value is not set then it will be tracked in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Added" /> state.
        ///         An entity is considered to have its primary key value set if the primary key property is set
        ///         to anything other than the CLR default for the property type.
        ///     </para>
        ///     <para>
        ///         Use <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State" /> to set the state of only a single entity.
        ///     </para>
        /// </summary>
        /// <param name="entity"> The entity to update. </param>
        /// <returns>
        ///     The <see cref="T:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry" /> for the entity. The entry provides
        ///     access to change tracking information and operations for the entity.
        /// </returns>
        EntityEntry Update(object entity);

        /// <summary>
        ///     Begins tracking the given entity in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Deleted" /> state such that it will
        ///     be removed from the database when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" /> is called.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         If the entity is already tracked in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Added" /> state then the context will
        ///         stop tracking the entity (rather than marking it as <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Deleted" />) since the
        ///         entity was previously added to the context and does not exist in the database.
        ///     </para>
        ///     <para>
        ///         Any other reachable entities that are not already being tracked will be tracked in the same way that
        ///         they would be if <see cref="M:Microsoft.EntityFrameworkCore.DbContext.Attach(System.Object)" /> was called before calling this method.
        ///         This allows any cascading actions to be applied when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" /> is called.
        ///     </para>
        ///     <para>
        ///         Use <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State" /> to set the state of only a single entity.
        ///     </para>
        /// </remarks>
        /// <param name="entity"> The entity to remove. </param>
        /// <returns>
        ///     The <see cref="T:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry" /> for the entity. The entry provides
        ///     access to change tracking information and operations for the entity.
        /// </returns>
        EntityEntry Remove(object entity);

        /// <summary>
        ///     Begins tracking the given entities, and any other reachable entities that are
        ///     not already being tracked, in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Added" /> state such that they will
        ///     be inserted into the database when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" /> is called.
        /// </summary>
        /// <param name="entities"> The entities to add. </param>
        void AddRange(params object[] entities);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity, and any other reachable entities that are
        ///         not already being tracked, in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Added" /> state such that they will
        ///         be inserted into the database when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" /> is called.
        ///     </para>
        ///     <para>
        ///         This method is async only to allow special value generators, such as the one used by
        ///         'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
        ///         to access the database asynchronously. For all other cases the non async method should be used.
        ///     </para>
        /// </summary>
        /// <param name="entities"> The entities to add. </param>
        /// <returns> A task that represents the asynchronous operation. </returns>
        Task AddRangeAsync(params object[] entities);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entities in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Unchanged" /> state
        ///         such that no operation will be performed when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" />
        ///         is called.
        ///     </para>
        ///     <para>
        ///         A recursive search of the navigation properties will be performed to find reachable entities
        ///         that are not already being tracked by the context. These entities will also begin to be tracked
        ///         by the context. If a reachable entity has its primary key value set
        ///         then it will be tracked in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Unchanged" /> state. If the primary key
        ///         value is not set then it will be tracked in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Added" /> state.
        ///         An entity is considered to have its primary key value set if the primary key property is set
        ///         to anything other than the CLR default for the property type.
        ///     </para>
        /// </summary>
        /// <param name="entities"> The entities to attach. </param>
        void AttachRange(params object[] entities);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entities in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Modified" /> state such that they will
        ///         be updated in the database when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" /> is called.
        ///     </para>
        ///     <para>
        ///         All properties of each entity will be marked as modified. To mark only some properties as modified, use
        ///         <see cref="M:Microsoft.EntityFrameworkCore.DbContext.Attach(System.Object)" /> to begin tracking each entity in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Unchanged" />
        ///         state and then use the returned <see cref="T:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry" /> to mark the desired properties as modified.
        ///     </para>
        ///     <para>
        ///         A recursive search of the navigation properties will be performed to find reachable entities
        ///         that are not already being tracked by the context. These entities will also begin to be tracked
        ///         by the context. If a reachable entity has its primary key value set
        ///         then it will be tracked in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Modified" /> state. If the primary key
        ///         value is not set then it will be tracked in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Added" /> state.
        ///         An entity is considered to have its primary key value set if the primary key property is set
        ///         to anything other than the CLR default for the property type.
        ///     </para>
        /// </summary>
        /// <param name="entities"> The entities to update. </param>
        void UpdateRange(params object[] entities);

        /// <summary>
        ///     Begins tracking the given entity in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Deleted" /> state such that it will
        ///     be removed from the database when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" /> is called.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         If any of the entities are already tracked in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Added" /> state then the context will
        ///         stop tracking those entities (rather than marking them as <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Deleted" />) since those
        ///         entities were previously added to the context and do not exist in the database.
        ///     </para>
        ///     <para>
        ///         Any other reachable entities that are not already being tracked will be tracked in the same way that
        ///         they would be if <see cref="M:Microsoft.EntityFrameworkCore.DbContext.AttachRange(System.Object[])" /> was called before calling this method.
        ///         This allows any cascading actions to be applied when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" /> is called.
        ///     </para>
        /// </remarks>
        /// <param name="entities"> The entities to remove. </param>
        void RemoveRange(params object[] entities);

        /// <summary>
        ///     Begins tracking the given entities, and any other reachable entities that are
        ///     not already being tracked, in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Added" /> state such that they will
        ///     be inserted into the database when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" /> is called.
        /// </summary>
        /// <param name="entities"> The entities to add. </param>
        void AddRange(IEnumerable<object> entities);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity, and any other reachable entities that are
        ///         not already being tracked, in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Added" /> state such that they will
        ///         be inserted into the database when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" /> is called.
        ///     </para>
        ///     <para>
        ///         This method is async only to allow special value generators, such as the one used by
        ///         'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
        ///         to access the database asynchronously. For all other cases the non async method should be used.
        ///     </para>
        /// </summary>
        /// <param name="entities"> The entities to add. </param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        /// </returns>
        Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entities in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Unchanged" /> state
        ///         such that no operation will be performed when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" />
        ///         is called.
        ///     </para>
        ///     <para>
        ///         A recursive search of the navigation properties will be performed to find reachable entities
        ///         that are not already being tracked by the context. These entities will also begin to be tracked
        ///         by the context. If a reachable entity has its primary key value set
        ///         then it will be tracked in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Unchanged" /> state. If the primary key
        ///         value is not set then it will be tracked in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Added" /> state.
        ///         An entity is considered to have its primary key value set if the primary key property is set
        ///         to anything other than the CLR default for the property type.
        ///     </para>
        /// </summary>
        /// <param name="entities"> The entities to attach. </param>
        void AttachRange(IEnumerable<object> entities);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entities in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Modified" /> state such that they will
        ///         be updated in the database when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" /> is called.
        ///     </para>
        ///     <para>
        ///         All properties of each entity will be marked as modified. To mark only some properties as modified, use
        ///         <see cref="M:Microsoft.EntityFrameworkCore.DbContext.Attach(System.Object)" /> to begin tracking each entity in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Unchanged" />
        ///         state and then use the returned <see cref="T:Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry" /> to mark the desired properties as modified.
        ///     </para>
        ///     <para>
        ///         A recursive search of the navigation properties will be performed to find reachable entities
        ///         that are not already being tracked by the context. These entities will also begin to be tracked
        ///         by the context. If a reachable entity has its primary key value set
        ///         then it will be tracked in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Modified" /> state. If the primary key
        ///         value is not set then it will be tracked in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Added" /> state.
        ///         An entity is considered to have its primary key value set if the primary key property is set
        ///         to anything other than the CLR default for the property type.
        ///     </para>
        /// </summary>
        /// <param name="entities"> The entities to update. </param>
        void UpdateRange(IEnumerable<object> entities);

        /// <summary>
        ///     Begins tracking the given entity in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Deleted" /> state such that it will
        ///     be removed from the database when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" /> is called.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         If any of the entities are already tracked in the <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Added" /> state then the context will
        ///         stop tracking those entities (rather than marking them as <see cref="F:Microsoft.EntityFrameworkCore.EntityState.Deleted" />) since those
        ///         entities were previously added to the context and do not exist in the database.
        ///     </para>
        ///     <para>
        ///         Any other reachable entities that are not already being tracked will be tracked in the same way that
        ///         they would be if <see cref="M:Microsoft.EntityFrameworkCore.DbContext.AttachRange(System.Collections.Generic.IEnumerable{System.Object})" /> was called before calling this method.
        ///         This allows any cascading actions to be applied when <see cref="M:Microsoft.EntityFrameworkCore.DbContext.SaveChanges" /> is called.
        ///     </para>
        /// </remarks>
        /// <param name="entities"> The entities to remove. </param>
        void RemoveRange(IEnumerable<object> entities);

        /// <summary>
        ///     Finds an entity with the given primary key values. If an entity with the given primary key values
        ///     is being tracked by the context, then it is returned immediately without making a request to the
        ///     database. Otherwise, a query is made to the database for an entity with the given primary key values
        ///     and this entity, if found, is attached to the context and returned. If no entity is found, then
        ///     null is returned.
        /// </summary>
        /// <param name="entityType"> The type of entity to find. </param>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <returns>The entity found, or null.</returns>
        object Find(Type entityType, params object[] keyValues);

        /// <summary>
        ///     Finds an entity with the given primary key values. If an entity with the given primary key values
        ///     is being tracked by the context, then it is returned immediately without making a request to the
        ///     database. Otherwise, a query is made to the database for an entity with the given primary key values
        ///     and this entity, if found, is attached to the context and returned. If no entity is found, then
        ///     null is returned.
        /// </summary>
        /// <param name="entityType"> The type of entity to find. </param>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <returns>The entity found, or null.</returns>
        Task<object> FindAsync(Type entityType, params object[] keyValues);

        /// <summary>
        ///     Finds an entity with the given primary key values. If an entity with the given primary key values
        ///     is being tracked by the context, then it is returned immediately without making a request to the
        ///     database. Otherwise, a query is made to the database for an entity with the given primary key values
        ///     and this entity, if found, is attached to the context and returned. If no entity is found, then
        ///     null is returned.
        /// </summary>
        /// <param name="entityType"> The type of entity to find. </param>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>The entity found, or null.</returns>
        Task<object> FindAsync(Type entityType, object[] keyValues, CancellationToken cancellationToken);

        /// <summary>
        ///     Finds an entity with the given primary key values. If an entity with the given primary key values
        ///     is being tracked by the context, then it is returned immediately without making a request to the
        ///     database. Otherwise, a query is made to the database for an entity with the given primary key values
        ///     and this entity, if found, is attached to the context and returned. If no entity is found, then
        ///     null is returned.
        /// </summary>
        /// <typeparam name="TEntity"> The type of entity to find. </typeparam>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <returns>The entity found, or null.</returns>
        TEntity Find<TEntity>(params object[] keyValues) where TEntity : class;

        /// <summary>
        ///     Finds an entity with the given primary key values. If an entity with the given primary key values
        ///     is being tracked by the context, then it is returned immediately without making a request to the
        ///     database. Otherwise, a query is made to the database for an entity with the given primary key values
        ///     and this entity, if found, is attached to the context and returned. If no entity is found, then
        ///     null is returned.
        /// </summary>
        /// <typeparam name="TEntity"> The type of entity to find. </typeparam>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <returns>The entity found, or null.</returns>
        Task<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class;

        /// <summary>
        ///     Finds an entity with the given primary key values. If an entity with the given primary key values
        ///     is being tracked by the context, then it is returned immediately without making a request to the
        ///     database. Otherwise, a query is made to the database for an entity with the given primary key values
        ///     and this entity, if found, is attached to the context and returned. If no entity is found, then
        ///     null is returned.
        /// </summary>
        /// <typeparam name="TEntity"> The type of entity to find. </typeparam>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>The entity found, or null.</returns>
        Task<TEntity> FindAsync<TEntity>(object[] keyValues, CancellationToken cancellationToken) where TEntity : class;

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns> A string that represents the current object. </returns>
        string ToString();

        /// <summary>
        ///     Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj"> The object to compare with the current object. </param>
        /// <returns> true if the specified object is equal to the current object; otherwise, false. </returns>
        bool Equals(object obj);

        /// <summary>Serves as the default hash function.</summary>
        /// <returns> A hash code for the current object. </returns>
        int GetHashCode();
    }
}