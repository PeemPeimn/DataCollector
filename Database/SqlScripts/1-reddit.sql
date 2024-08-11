CREATE TABLE reddit_posts(
  id text PRIMARY KEY,
  created_at timestamp with time zone NOT NULL,
  post jsonb NOT NULL
);

CREATE TABLE reddit_data(
  id serial PRIMARY KEY,
  post_id text REFERENCES reddit_posts(id) ON DELETE CASCADE,
  created_at timestamp with time zone NOT NULL,
  conversation jsonb NOT NULL
);

