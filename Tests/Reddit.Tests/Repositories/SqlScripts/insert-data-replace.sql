INSERT INTO reddit_posts(id, created_at, post)
  VALUES ('test_post_id', NOW(), '{"test": "test"}');

INSERT INTO reddit_data(post_id, created_at, messages)
  VALUES ('test_post_id', NOW(), '{"test": "test"}'),
('test_post_id', NOW(), '{"test": "test"}'),
('test_post_id', NOW(), '{"test": "test"}');

