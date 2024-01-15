﻿using Elima.Common.Domain.Entities;
using Elima.Common.Domain.Entities.Auditing.Contracts;
using Elima.Common.EntityFramework.EntityFrameworkCore.Modeling;
using Elima.Common.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elima.Common.EntityFramework.EntityFrameworkCore.Modeling;

public static class EntityTypeBuilderExtensions
{
    public static void ConfigureByConvention(this EntityTypeBuilder b)
    {
        b.TryConfigureConcurrencyStamp();
        b.TryConfigureMayHaveCreator();
        b.TryConfigureMustHaveCreator();
        b.TryConfigureSoftDelete();
        b.TryConfigureDeletionTime();
        b.TryConfigureDeletionAudited();
        b.TryConfigureCreationTime();
        b.TryConfigureLastModificationTime();
        b.TryConfigureModificationAudited();
    }

    public static void ConfigureConcurrencyStamp<T>(this EntityTypeBuilder<T> b)
        where T : class, IHasConcurrencyStamp
    {
        b.As<EntityTypeBuilder>().TryConfigureConcurrencyStamp();
    }

    public static void TryConfigureConcurrencyStamp(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IHasConcurrencyStamp>())
        {
            b.Property(nameof(IHasConcurrencyStamp.ConcurrencyStamp))
                .IsConcurrencyToken()
                .HasMaxLength(ConcurrencyStampConsts.MaxLength)
                .HasColumnName(nameof(IHasConcurrencyStamp.ConcurrencyStamp));
        }
    }

    public static void ConfigureSoftDelete<T>(this EntityTypeBuilder<T> b)
        where T : class, ISoftDelete
    {
        b.As<EntityTypeBuilder>().TryConfigureSoftDelete();
    }

    public static void TryConfigureSoftDelete(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<ISoftDelete>())
        {
            b.Property(nameof(ISoftDelete.IsDeleted))
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName(nameof(ISoftDelete.IsDeleted));
        }
    }

    public static void ConfigureDeletionTime<T>(this EntityTypeBuilder<T> b)
        where T : class, IHasDeletionTime
    {
        b.As<EntityTypeBuilder>().TryConfigureDeletionTime();
    }

    public static void TryConfigureDeletionTime(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IHasDeletionTime>())
        {
            b.TryConfigureSoftDelete();

            b.Property(nameof(IHasDeletionTime.DeletionTime))
                .IsRequired(false)
                .HasColumnName(nameof(IHasDeletionTime.DeletionTime));
        }
    }

    public static void ConfigureMayHaveCreator<T>(this EntityTypeBuilder<T> b)
        where T : class, IMayHaveCreator
    {
        b.As<EntityTypeBuilder>().TryConfigureMayHaveCreator();
    }

    public static void TryConfigureMayHaveCreator(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IMayHaveCreator>())
        {
            b.Property(nameof(IMayHaveCreator.CreatorId))
                .IsRequired(false)
                .HasColumnName(nameof(IMayHaveCreator.CreatorId));
        }
    }

    public static void ConfigureMustHaveCreator<T>(this EntityTypeBuilder<T> b)
        where T : class, IMustHaveCreator
    {
        b.As<EntityTypeBuilder>().TryConfigureMustHaveCreator();
    }

    public static void TryConfigureMustHaveCreator(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IMustHaveCreator>())
        {
            b.Property(nameof(IMustHaveCreator.CreatorId))
                .IsRequired()
                .HasColumnName(nameof(IMustHaveCreator.CreatorId));
        }
    }

    public static void ConfigureDeletionAudited<T>(this EntityTypeBuilder<T> b)
        where T : class, IDeletionAuditedObject
    {
        b.As<EntityTypeBuilder>().TryConfigureDeletionAudited();
    }

    public static void TryConfigureDeletionAudited(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IDeletionAuditedObject>())
        {
            b.TryConfigureDeletionTime();

            b.Property(nameof(IDeletionAuditedObject.DeleterId))
                .IsRequired(false)
                .HasColumnName(nameof(IDeletionAuditedObject.DeleterId));
        }
    }

    public static void ConfigureCreationTime<T>(this EntityTypeBuilder<T> b)
        where T : class, IHasCreationTime
    {
        b.As<EntityTypeBuilder>().TryConfigureCreationTime();
    }

    public static void TryConfigureCreationTime(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IHasCreationTime>())
        {
            b.Property(nameof(IHasCreationTime.CreationTime))
                .IsRequired()
                .HasColumnName(nameof(IHasCreationTime.CreationTime));
        }
    }

    public static void ConfigureCreationAudited<T>(this EntityTypeBuilder<T> b)
        where T : class, ICreationAuditedObject
    {
        b.As<EntityTypeBuilder>().TryConfigureCreationAudited();
    }

    public static void TryConfigureCreationAudited(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<ICreationAuditedObject>())
        {
            b.As<EntityTypeBuilder>().TryConfigureCreationTime();
            b.As<EntityTypeBuilder>().TryConfigureMayHaveCreator();
        }
    }

    public static void ConfigureLastModificationTime<T>(this EntityTypeBuilder<T> b)
        where T : class, IHasModificationTime
    {
        b.As<EntityTypeBuilder>().TryConfigureLastModificationTime();
    }

    public static void TryConfigureLastModificationTime(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IHasModificationTime>())
        {
            b.Property(nameof(IHasModificationTime.LastModificationTime))
                .IsRequired(false)
                .HasColumnName(nameof(IHasModificationTime.LastModificationTime));
        }
    }

    public static void ConfigureModificationAudited<T>(this EntityTypeBuilder<T> b)
        where T : class, IModificationAuditedObject
    {
        b.As<EntityTypeBuilder>().TryConfigureModificationAudited();
    }

    public static void TryConfigureModificationAudited(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IModificationAuditedObject>())
        {
            b.TryConfigureLastModificationTime();

            b.Property(nameof(IModificationAuditedObject.LastModifierId))
                .IsRequired(false)
                .HasColumnName(nameof(IModificationAuditedObject.LastModifierId));
        }
    }

    public static void ConfigureAudited<T>(this EntityTypeBuilder<T> b)
        where T : class, IAuditedObject
    {
        b.As<EntityTypeBuilder>().TryConfigureAudited();
    }

    public static void TryConfigureAudited(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IAuditedObject>())
        {
            b.As<EntityTypeBuilder>().TryConfigureCreationAudited();
            b.As<EntityTypeBuilder>().TryConfigureModificationAudited();
        }
    }

    public static void ConfigureFullAudited<T>(this EntityTypeBuilder<T> b)
        where T : class, IFullAuditedObject
    {
        b.As<EntityTypeBuilder>().TryConfigureFullAudited();
    }

    public static void TryConfigureFullAudited(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IFullAuditedObject>())
        {
            b.As<EntityTypeBuilder>().TryConfigureAudited();
            b.As<EntityTypeBuilder>().TryConfigureDeletionAudited();
        }
    }

    public static void ConfigureCreationAuditedAggregateRoot<T>(this EntityTypeBuilder<T> b)
        where T : class
    {
        b.As<EntityTypeBuilder>().TryConfigureCreationAudited();
        b.As<EntityTypeBuilder>().TryConfigureConcurrencyStamp();
    }

    public static void ConfigureAuditedAggregateRoot<T>(this EntityTypeBuilder<T> b)
        where T : class
    {
        b.As<EntityTypeBuilder>().TryConfigureAudited();
        b.As<EntityTypeBuilder>().TryConfigureConcurrencyStamp();
    }

    public static void ConfigureFullAuditedAggregateRoot<T>(this EntityTypeBuilder<T> b)
        where T : class
    {
        b.As<EntityTypeBuilder>().TryConfigureFullAudited();
        b.As<EntityTypeBuilder>().TryConfigureConcurrencyStamp();
    }

    //TODO: Add other interfaces (IAuditedObject<TUser>...)
}
