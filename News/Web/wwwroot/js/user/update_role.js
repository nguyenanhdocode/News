$(document).ready(() => {

	$('#userRoles option:selected').prop('selected', false);

	$('#btnAdd').bind('click', function (e) {
		e.preventDefault();
		const perms = $('#roles').find(":selected").get();
		if (perms && perms.length > 0) {
			const rolePerms = $('#userRoles');
			rolePerms.append(...perms);
			$('#roles').remove(...perms);
		}
	});

	$('#btnAddAll').bind('click', function (e) {
		e.preventDefault();
		const perms = $('#roles option').get();
		if (perms && perms.length > 0) {
			const rolePerms = $('#userRoles');
			rolePerms.append(...perms);
			$('#userRoles').remove(...perms);
		}
	});

	$('#btnRemove').bind('click', function (e) {
		e.preventDefault();
		const rolePerms = $('#userRoles').find(":selected").get();
		if (rolePerms && rolePerms.length > 0) {
			const perms = $('#roles');
			perms.append(...rolePerms);
			$('#userRoles').remove(...rolePerms);
		}
	});

	$('#btnRemoveAll').bind('click', function (e) {
		e.preventDefault();
		const rolePerms = $('#userRoles option').get();
		if (rolePerms && rolePerms.length > 0) {
			const perms = $('#roles');
			perms.append(...rolePerms);
			$('#userRoles').remove(...rolePerms);
		}
	});

	$('#btnSubmit').bind('click', function (e) {
		$('#userRoles option').not(':selected').prop('selected', 'true');
	});
});