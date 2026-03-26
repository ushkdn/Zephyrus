CREATE TABLE IF NOT EXISTS users
(
    id            UUID         PRIMARY KEY,
    email         VARCHAR(256) NOT NULL UNIQUE,
    password      TEXT         NOT NULL,
    first_name    VARCHAR(100) NOT NULL,
    middle_name   VARCHAR(100) NOT NULL,
    last_name     VARCHAR(100) NOT NULL,
    role          SMALLINT     NOT NULL,
    is_active     BOOLEAN      NOT NULL DEFAULT TRUE,
    date_created  TIMESTAMPTZ  NOT NULL,
    date_updated  TIMESTAMPTZ  NOT NULL
);
 