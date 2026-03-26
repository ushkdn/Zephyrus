CREATE TABLE IF NOT EXISTS refresh_tokens
(
    id           UUID        PRIMARY KEY,
    user_id      UUID        NOT NULL REFERENCES users (id) ON DELETE CASCADE,
    token        TEXT        NOT NULL UNIQUE,
    date_expires TIMESTAMPTZ NOT NULL,
    date_created TIMESTAMPTZ NOT NULL,
    date_updated TIMESTAMPTZ NOT NULL
);
 
CREATE INDEX IF NOT EXISTS ix_refresh_tokens_token
    ON refresh_tokens (token);
 
CREATE INDEX IF NOT EXISTS ix_refresh_tokens_user_id
    ON refresh_tokens (user_id);
 