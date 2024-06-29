CREATE TABLE reddit_posts(
  id integer PRIMARY KEY,
  created_at timestamp with time zone NOT NULL,
  post jsonb NOT NULL
);

CREATE TABLE reddit_data(
  id serial PRIMARY KEY,
  post_id integer REFERENCES reddit_posts(id),
  created_at timestamp with time zone NOT NULL,
  messages jsonb NOT NULL
);

