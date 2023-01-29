function count()
{
    $files = ls

    $total = 0;

    for ($i = 0; $i -le $files.Length; $i += 1)
    {
        if ($files -ne $null)
        {
            if ($files[$i].Extension -eq ".cs")
            {
                $total += (Get-content $files[$i]).Length
            }
            $path = $files[$i].Name;
            if ($path -ne $null -and (Get-Item $path) -is [System.IO.DirectoryInfo])
            {
                cd $files[$i]
                $total += count
                cd ..
            }
        }
    }

    return $total
}

count