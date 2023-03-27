# Sqlite connection

    builder.Services.AddDbContext<DevEventsDbContext>(
        x => x.UseSqlite("Data source=DevEvents.db")
    );
