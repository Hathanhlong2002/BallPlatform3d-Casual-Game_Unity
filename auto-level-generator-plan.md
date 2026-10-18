# Auto Level Generator Plan (BallPlatform3D)

## Goal
Xây dựng công cụ Unity Editor Tool sinh Level tự động (Procedural / Modular Chunk-based Level Generator) từ Level 16 đến Level 30+, tuân thủ đúng chuẩn prefab, tag, collider và gameplay loop của 15 level gốc.

## Core Findings from Original Levels (Level 1 - 15)
1. **Trục di chuyển chính**: Đi dọc theo trục **Z** (Z bắt đầu từ ~0 đến 50-120 tùy độ dài level), biên độ ngang **X** từ -6 đến +6.
2. **Kiến trúc Scene bắt buộc**:
   - `GameManager.prefab`: Chứa GameManager, AudioManager, ItemCollector, LoadBalls, LoadWorld, Next, SpawnCoins.
   - `Canvas.prefab`: HUD UI (Coins, Heart, Joystick/Controls).
   - `Start.prefab` + `SpawnPoint`: Điểm xuất phát của bóng.
   - `SpawnWorldPos`: Điểm render background thế giới.
   - `MainCamera` & `Directional Light`.
   - `Ground` & `Bridge` Containers: Chứa các modular blocks được gán đúng Tag (`Ground`, `Bridge`) để script `SpawnCoins.cs` tự động rải coin khi Play.
   - `NextLevel.prefab`: Đích đến (gồm 2 cánh cửa xoay + collider trigger `Finish.cs`).
3. **Chướng ngại vật & Cơ chế nâng cao**:
   - Chướng ngại vật tĩnh: `Hurdle01`, `Hurdle02`.
   - Platform di động: Gán `WaypointFollow.cs` + 2 Transform waypoints (tối ưu bằng Kinematic Rigidbody).
   - Chướng ngại vật xoay: Gán `Rotate.cs`.
   - Vực chết: Vùng rơi ra ngoài va chạm sàn `BackGround` kích hoạt `Die()`.

## Tasks
- [x] Task 1: Thiết kế cấu hình sinh level theo modular chunks và độ khó.
- [x] Task 2: Định nghĩa thư viện Modular Chunks (Start, Bridge Straight, S-Curve, Moving Platform gap, Midpoint Island Checkpoint, Hurdle Obstacles, Finish Goal).
- [x] Task 3: Viết công cụ `LevelGenerator.cs` ([Assets/Editor/LevelGenerator.cs](file:///Users/thanhlong/Documents/Game/untitled%20folder/BallPlatform3d-Casual-Game_Unity/Assets/Editor/LevelGenerator.cs)) hỗ trợ cả GUI Editor Window và CLI BatchMode.
- [x] Task 4: Tự động cập nhật `EditorBuildSettings.asset` khi tạo scene mới (đã đăng ký `Level 30.unity`).
- [x] Task 5: Kiểm tra tính chơi được và hierarchy của `Level 30.unity` (28 prefabs liên kết, camera follow, audio listener, void killplane, auto-coin spawner).

## Done When
- [x] Đã tạo thành công `Level 30.unity` lấp đầy khoảng trống thiếu sót của project.
- [x] Tích hợp menu `Tools -> Ball Game -> Level Generator Window` trên Unity Editor để designer có thể tạo thêm bất kỳ level nào tiếp theo với 1-click.

