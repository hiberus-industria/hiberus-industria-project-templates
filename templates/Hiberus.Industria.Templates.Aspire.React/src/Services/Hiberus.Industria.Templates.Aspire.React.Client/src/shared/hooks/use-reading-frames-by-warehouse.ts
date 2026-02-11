import type { ProblemDetails, ReadingFrameWithDetailsDto } from "@/client";
import { getReadingFramesOptions } from "@/client/@tanstack/react-query.gen";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { useMemo } from "react";

const DEFAULT_PAGE_SIZE = 20;

// Interface for the hook's return type
interface ReadingFramesResult {
    data: ReadingFrameWithDetailsDto[];
    totalCount?: number;
    totalPages?: number;
    isLoading: boolean;
    isFetching: boolean;
    error: Error | ProblemDetails | null;
    invalidateQueries: () => void;
}

/**
 * Custom hook to fetch reading frames data filtered by warehouse.
 * @param warehouseId - Array of warehouse IDs to filter by
 * @returns ReadingFramesResult containing data, pagination info, and query controls.
 */
export function useReadingFramesByWarehouse(
    warehouseId: number[],
): ReadingFramesResult {
    const queryClient = useQueryClient();

    const validatedFilters = useMemo(
        () => ({
            page: 1,
            pageSize: DEFAULT_PAGE_SIZE,
            warehouseId,
        }),
        [warehouseId],
    );

    const queryOptions = getReadingFramesOptions({ query: validatedFilters });
    const queryKey = queryOptions.queryKey;

    const queryResult = useQuery({
        ...queryOptions,
        placeholderData: (previous) => previous,
        staleTime: 1000 * 60 * 5, // 5 minutes
    });

    const invalidateQueries = () => {
        queryClient.invalidateQueries({ queryKey });
    };

    return {
        data: queryResult.data?.items ?? [],
        totalCount: queryResult.data?.totalCount,
        totalPages: queryResult.data?.totalPages,
        isLoading: queryResult.isLoading,
        isFetching: queryResult.isFetching,
        error: queryResult.error,
        invalidateQueries,
    };
}
