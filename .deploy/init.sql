CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20231118162952_AddedUsers') THEN
    CREATE TYPE role AS ENUM ('user', 'moderator', 'admin');
    CREATE TYPE vote_type AS ENUM ('wish_vote', 'comment_vote');
    CREATE TYPE wish_status AS ENUM ('moderation', 'in_process', 'rejected', 'completed');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20231118162952_AddedUsers') THEN
    CREATE TABLE users (
        id uuid NOT NULL,
        username text NOT NULL,
        first_name text NOT NULL,
        second_name text NOT NULL,
        email text NOT NULL,
        role role NOT NULL,
        avatar text NULL,
        google_name_id text NOT NULL,
        CONSTRAINT pk_users PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20231118162952_AddedUsers') THEN
    CREATE UNIQUE INDEX ix_email_1 ON users (email);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20231118162952_AddedUsers') THEN
    CREATE UNIQUE INDEX ix_google_name_id_1 ON users (google_name_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20231118162952_AddedUsers') THEN
    CREATE UNIQUE INDEX ix_username_1 ON users (username);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20231118162952_AddedUsers') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20231118162952_AddedUsers', '7.0.11');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20231211093339_AddedWishesAndProducts') THEN
    CREATE TABLE products (
        id text NOT NULL,
        name text NOT NULL,
        logo text NOT NULL,
        CONSTRAINT pk_products PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20231211093339_AddedWishesAndProducts') THEN
    CREATE TABLE wishes (
        id uuid NOT NULL,
        created_at timestamp with time zone NOT NULL,
        status wish_status NOT NULL,
        comment_count integer NOT NULL,
        vote_count integer NOT NULL,
        is_deleted boolean NOT NULL,
        is_visible_for_users boolean NOT NULL,
        last_update_at timestamp with time zone NOT NULL,
        text text NOT NULL,
        product_id text NOT NULL,
        creator_id uuid NOT NULL,
        CONSTRAINT pk_wishes PRIMARY KEY (id),
        CONSTRAINT fk_wishes_products_product_id FOREIGN KEY (product_id) REFERENCES products (id) ON DELETE CASCADE,
        CONSTRAINT fk_wishes_users_creator_temp_id FOREIGN KEY (creator_id) REFERENCES users (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20231211093339_AddedWishesAndProducts') THEN
    CREATE UNIQUE INDEX ix_name_1 ON products (name);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20231211093339_AddedWishesAndProducts') THEN
    CREATE INDEX ix_creator_id_1 ON wishes (creator_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20231211093339_AddedWishesAndProducts') THEN
    CREATE INDEX ix_product_id_1 ON wishes (product_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20231211093339_AddedWishesAndProducts') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20231211093339_AddedWishesAndProducts', '7.0.11');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20231211184316_AddedComments') THEN
    ALTER TYPE wish_status RENAME VALUE 'moderation' TO 'moderating';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20231211184316_AddedComments') THEN
    ALTER TYPE wish_status RENAME VALUE 'in_process' TO 'processing';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20231211184316_AddedComments') THEN
    CREATE TABLE comments (
        id uuid NOT NULL,
        wish_id uuid NOT NULL,
        creator_id uuid NOT NULL,
        creator_role role NOT NULL,
        created_at timestamp with time zone NOT NULL,
        last_update_at timestamp with time zone NOT NULL,
        vote_count bigint NOT NULL,
        text text NOT NULL,
        files jsonb NOT NULL,
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_comments PRIMARY KEY (id),
        CONSTRAINT fk_comments_users_comment_creator_id FOREIGN KEY (creator_id) REFERENCES users (id) ON DELETE CASCADE,
        CONSTRAINT fk_comments_wishes_wish_id FOREIGN KEY (wish_id) REFERENCES wishes (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20231211184316_AddedComments') THEN
    CREATE INDEX ix_comments_creator_id ON comments (creator_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20231211184316_AddedComments') THEN
    CREATE INDEX ix_comments_wish_id ON comments (wish_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20231211184316_AddedComments') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20231211184316_AddedComments', '7.0.11');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20231222182745_AddedVotes') THEN
    CREATE TABLE votes (
        entity_id uuid NOT NULL,
        type vote_type NOT NULL,
        creator_id uuid NOT NULL,
        CONSTRAINT pk_votes PRIMARY KEY (creator_id, type, entity_id),
        CONSTRAINT fk_votes_users_creator_temp_id1 FOREIGN KEY (creator_id) REFERENCES users (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20231222182745_AddedVotes') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20231222182745_AddedVotes', '7.0.11');
    END IF;
END $EF$;
COMMIT;

