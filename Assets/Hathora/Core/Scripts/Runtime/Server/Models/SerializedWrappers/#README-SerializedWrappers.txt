This dir exists because SDK models do not have a backing field, so does not count as serializable from Unity's perspective. Eg: We cannot use it for Editor script persistence.

Therefore, we use the middleware wrappers for HathoraServerConfig Editor persistence.